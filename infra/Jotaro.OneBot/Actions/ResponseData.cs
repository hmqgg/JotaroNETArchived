using System.Text.Json.Serialization;
using Jotaro.Utils.Converters;
using MessagePack;

namespace Jotaro.OneBot.Actions;

[MessagePackObject]
public record ResponseSendMessage([property: JsonPropertyName("message_id"), Key("message_id")] string MessageId,
    [property: JsonPropertyName("time"), Key("time"), JsonConverter(typeof(JsonUnixTimestampConverter)),
               MessagePackFormatter(typeof(MsgPackUnixTimestampFormatter))]
    DateTimeOffset Time);
