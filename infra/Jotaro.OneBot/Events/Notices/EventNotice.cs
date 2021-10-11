using System.Text.Json.Serialization;

namespace Jotaro.OneBot.Events.Notices;

public record EventNotice(string SubType, string Uuid, string SelfId, string Platform, DateTimeOffset Time) : Event(
    EventSet.Notice, SubType, Uuid, SelfId, Platform, Time)
{
    internal const string EventTypeName = "notice";
}

public record EventNoticeGroupIncrease([property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("group_id")] string GroupId, string SubType, string Uuid, string SelfId,
    string Platform, DateTimeOffset Time) : Event(EventSet.NoticeGroupIncrease, SubType, Uuid, SelfId, Platform, Time);

public record EventNoticeGroupDecrease([property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("group_id")] string GroupId, string SubType, string Uuid, string SelfId,
    string Platform, DateTimeOffset Time) : Event(EventSet.NoticeGroupDecrease, SubType, Uuid, SelfId, Platform, Time);

public record EventNoticeGroupAdmin([property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("group_id")] string GroupId, string SubType, string Uuid, string SelfId,
    string Platform, DateTimeOffset Time) : Event(EventSet.NoticeGroupAdmin, SubType, Uuid, SelfId, Platform, Time);

public record EventNoticeGroupBan([property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("group_id")] string GroupId, string SubType, string Uuid, string SelfId,
    string Platform, DateTimeOffset Time) : Event(EventSet.NoticeGroupBan, SubType, Uuid, SelfId, Platform, Time);

public record EventNoticeFriend([property: JsonPropertyName("user_id")] string UserId, string SubType, string Uuid,
    string SelfId, string Platform, DateTimeOffset Time) : Event(EventSet.NoticeFriend, SubType, Uuid, SelfId, Platform,
    Time);

public record EventNoticeGroupMessageDelete([property: JsonPropertyName("message_id")] string MessageId,
    [property: JsonPropertyName("group_id")] string GroupId, string SubType, string Uuid, string SelfId,
    string Platform, DateTimeOffset Time) : Event(EventSet.NoticeGroupMessageDelete, SubType, Uuid, SelfId, Platform,
    Time);

public record EventNoticePrivateMessageDelete([property: JsonPropertyName("message_id")] string MessageId,
    [property: JsonPropertyName("user_id")] string UserId, string SubType, string Uuid, string SelfId, string Platform,
    DateTimeOffset Time) : Event(EventSet.NoticePrivateMessageDelete, SubType, Uuid, SelfId, Platform, Time);
