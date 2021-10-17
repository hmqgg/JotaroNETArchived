using System.Text.Json.Serialization;
using Jotaro.OneBot.Segments;
using MessagePack;

namespace Jotaro.OneBot.Actions;

[MessagePackObject]
public record ParamsSendMessage([property: JsonPropertyName("detail_type"), Key("detail_type")] string DetailType,
    [property: JsonPropertyName("message"), Key("message")] IList<Segment> Message,
    [property: JsonPropertyName("user_id"), Key("user_id")] string UserId = "",
    [property: JsonPropertyName("group_id"), Key("group_id")] string GroupId = "") : ParamsData;

[MessagePackObject]
public record ParamsSendMessagePrivate(string UserId, IList<Segment> Message) : ParamsSendMessage("private", Message,
    UserId);

[MessagePackObject]
public record ParamsSendMessageGroup(string GroupId, IList<Segment> Message) : ParamsSendMessage("group", Message,
    GroupId: GroupId);

[MessagePackObject]
public record ParamsDeleteMessage
    ([property: JsonPropertyName("message_id"), Key("message_id")] string MessageId) : ParamsData;
