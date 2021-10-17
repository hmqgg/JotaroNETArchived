using System.Text.Json.Serialization;
using MessagePack;

namespace Jotaro.OneBot.Actions;

[JsonConverter(typeof(JsonActionRequestConverter)), MessagePackFormatter(typeof(MsgPackActionRequestFormatter<,>))]
public abstract record ActionRequest<TReqParams, TRespData>(ActionType Action, TReqParams Params, string Echo = "")
    where TReqParams : ParamsData where TRespData : class
{
    /// <summary>
    ///     As a type marker.
    /// </summary>
    [JsonIgnore, IgnoreMember]
    internal TRespData? ResponseData { get; set; }
}

public record ActionRequestUnknown(ParamsUnknown Params, string Echo = "") : ActionRequest<ParamsUnknown, object>(
    ActionType.Unknown, Params, Echo);

public record ActionRequestSendMessage(ParamsSendMessage Params, string Echo = "") :
    ActionRequest<ParamsSendMessage, ResponseSendMessage>(ActionType.SendMessage, Params, Echo);
