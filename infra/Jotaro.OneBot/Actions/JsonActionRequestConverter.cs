using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jotaro.OneBot.Actions;

public sealed class JsonActionRequestConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        // Cannot convert if is not `ActionRequest<,>`.
        if (!typeToConvert.IsGenericType || typeToConvert.GetGenericTypeDefinition() != typeof(ActionRequest<,>))
        {
            return false;
        }

        // All generica args must be class.
        return typeof(ParamsData).IsAssignableFrom(typeToConvert.GetGenericArguments()[0]) &&
               typeToConvert.GetGenericArguments()[1].IsClass;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var paramsType = typeToConvert.GetGenericArguments()[0];
        var respType = typeToConvert.GetGenericArguments()[1];

        return (JsonConverter?)Activator.CreateInstance(
            typeof(JsonActionRequestConverterInner<,>).MakeGenericType(paramsType, respType),
            BindingFlags.Instance | BindingFlags.Public, null, null, null);
    }

    private class
        JsonActionRequestConverterInner<TReqParams, TRespData> : JsonConverter<ActionRequest<TReqParams, TRespData>>
        where TReqParams : ParamsData where TRespData : class
    {
        public override ActionRequest<TReqParams, TRespData> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (JsonDocument.TryParseValue(ref reader, out var doc) &&
                doc.RootElement.TryGetProperty(ConstantsOfAction.sActionPropertyName, out var actionProperty))
            {
                var action = actionProperty.Deserialize<ActionType>(options) ?? ActionType.Unknown with
                {
                    // Call `GetString()` instead of `GetRawText()` to prevent redundant quotes.
                    Key = actionProperty.GetString() ?? string.Empty
                };

                var result = doc.Deserialize(action.ClassType, options);
                if (result is ActionRequest<TReqParams, TRespData> req)
                {
                    return req;
                }

                throw new NotSupportedException(
                    $"{nameof(ActionType)} {actionProperty.GetRawText()} is not supported by {nameof(JsonActionRequestConverter)}.");
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ActionRequest<TReqParams, TRespData> value,
            JsonSerializerOptions options)
        {
            // {
            writer.WriteStartObject();

            // "action":"send_message",
            writer.WritePropertyName(ConstantsOfAction.sActionPropertyName);
            JsonSerializer.Serialize(writer, value.Action, options);

            // "params":{},
            writer.WritePropertyName(ConstantsOfAction.sParamsPropertyName);
            JsonSerializer.Serialize(writer, value.Params, options);

            // Only if present.
            // "echo": "1234"
            if (!string.IsNullOrEmpty(value.Echo))
            {
                writer.WritePropertyName(ConstantsOfAction.sEchoPropertyName);
                JsonSerializer.Serialize(writer, value.Echo, options);
            }

            // }
            writer.WriteEndObject();
        }
    }
}
