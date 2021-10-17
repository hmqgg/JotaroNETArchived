using System.Buffers;
using System.Text;
using MessagePack;
using MessagePack.Formatters;

namespace Jotaro.OneBot.Actions;

public sealed class
    MsgPackActionRequestFormatter<TReqParams, TRespData> : IMessagePackFormatter<ActionRequest<TReqParams, TRespData>>
    where TReqParams : ParamsData where TRespData : class
{
    public void Serialize(ref MessagePackWriter writer, ActionRequest<TReqParams, TRespData> value,
        MessagePackSerializerOptions options)
    {
        var hasEcho = string.IsNullOrEmpty(value.Echo);
        var elementCount = hasEcho ? 2 : 3;

        // 2/3-element map, with echo or not.
        writer.WriteMapHeader(elementCount);

        // 6-byte string `action`.
        writer.WriteString(ConstantsOfAction.sActionPropertyName);
        // n-byte string e.g. `send_message`.
        MessagePackSerializer.Serialize(ref writer, value.Action, options);

        // 6-byte string `params`.
        writer.WriteString(ConstantsOfAction.sParamsPropertyName);
        // Params object.
        MessagePackSerializer.Serialize(ref writer, value.Params, options);

        if (hasEcho)
        {
            // 4-byte string `echo`.
            writer.WriteString(ConstantsOfAction.sEchoPropertyName);
            MessagePackSerializer.Serialize(ref writer, value.Echo, options);
        }
    }

    public ActionRequest<TReqParams, TRespData> Deserialize(ref MessagePackReader reader,
        MessagePackSerializerOptions options)
    {
        if (!reader.TryReadMapHeader(out var elementCount))
        {
            throw new MessagePackSerializationException();
        }

        var actionSeq = ReadOnlySequence<byte>.Empty;
        var paramsSeq = ReadOnlySequence<byte>.Empty;
        var echo = string.Empty;

        // `elementCount` shall be 2 or 3.
        for (var i = 0; i < elementCount; i++)
        {
            if (reader.TryReadStringSpan(out var propertyName))
            {
                if (propertyName.SequenceEqual(ConstantsOfAction.sActionPropertyName))
                {
                    actionSeq = reader.ReadRaw();
                }
                else if (propertyName.SequenceEqual(ConstantsOfAction.sParamsPropertyName))
                {
                    paramsSeq = reader.ReadRaw();
                }
                else if (propertyName.SequenceEqual(ConstantsOfAction.sEchoPropertyName))
                {
                    echo = reader.ReadString();
                }
            }
        }

        var action = MessagePackSerializer.Deserialize<ActionType>(actionSeq, options) ?? ActionType.Unknown with
        {
            Key = MessagePackSerializer.Deserialize<string>(actionSeq, options)
        };

        var param = MessagePackSerializer.Deserialize(typeof(TReqParams), paramsSeq, options);

        // Full-trust `ActionType`, so `GetUninitializedObject` is safe, but it is f**king SLOW.
        // var zeroObj = FormatterServices.GetUninitializedObject(action.ClassType);
        // `Activator` is much (4x-6x) faster, but it depends on the constructor.
        var zeroObj = Activator.CreateInstance(action.ClassType, param, echo);
        if (zeroObj is ActionRequest<TReqParams, TRespData> req && param is TReqParams reqParam)
        {
            return req with { Action = action, Params = reqParam, Echo = echo };
        }

        // Failed in deserialize.
        throw new NotSupportedException(
            $"{nameof(ActionType)} {Encoding.UTF8.GetString(actionSeq)} is not supported by {nameof(MsgPackActionRequestFormatter<TReqParams, TRespData>)}.");
    }
}
