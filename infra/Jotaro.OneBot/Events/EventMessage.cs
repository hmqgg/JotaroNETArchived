using System.Text.Json.Serialization;
using Jotaro.OneBot.Segments;

namespace Jotaro.OneBot.Events;

public record EventMessage(string SubType, string Uuid, string SelfId, string Platform, DateTimeOffset Time) : Event(
    EventSet.Message, SubType, Uuid, SelfId, Platform, Time)
{
    internal const string EventTypeName = "message";
}

public record EventMessagePrivate([property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("message")] IList<Segment> Message,
    [property: JsonPropertyName("message_id")] string MessageId,
    [property: JsonPropertyName("alt_message")] string AltMessage, string SubType, string Uuid, string SelfId,
    string Platform, DateTimeOffset Time) : Event(EventSet.MessagePrivate, SubType, Uuid, SelfId, Platform, Time);

public record EventMessageGroup([property: JsonPropertyName("user_id")] string UserId,
    [property: JsonPropertyName("group_id")] string GroupId,
    [property: JsonPropertyName("message")] IList<Segment> Message,
    [property: JsonPropertyName("message_id")] string MessageId,
    [property: JsonPropertyName("alt_message")] string AltMessage, string SubType, string Uuid, string SelfId,
    string Platform, DateTimeOffset Time) : Event(EventSet.MessageGroup, SubType, Uuid, SelfId, Platform, Time);
