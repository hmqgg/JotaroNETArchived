using System.Text.Json.Serialization;
using MessagePack;

namespace Jotaro.OneBot.Segments;

public abstract record SegmentData
{
    [JsonExtensionData, IgnoreMember]
    public Dictionary<string, object> ExtensionData { get; init; } = new();
}

[MessagePackObject]
public record SegmentUnknown : SegmentData;

[MessagePackObject]
public record SegmentText([property: JsonPropertyName("text"), Key("text")] string Text) : SegmentData;

// TODO: Segment with `file_id`.
[MessagePackObject]
public record SegmentImage([property: JsonPropertyName("file_id"), Key("file_id")] string FileId) : SegmentData;

[MessagePackObject]
public record SegmentVoice([property: JsonPropertyName("file_id"), Key("file_id")] string FileId) : SegmentData;

[MessagePackObject]
public record SegmentFile([property: JsonPropertyName("file_id"), Key("file_id")] string FileId) : SegmentData;

[MessagePackObject]
public record SegmentMention([property: JsonPropertyName("user_id"), Key("user_id")] string UserId) : SegmentData;

[MessagePackObject]
public record SegmentVideo([property: JsonPropertyName("file_id"), Key("file_id")] string FileId) : SegmentData;

[MessagePackObject]
public record SegmentLocation([property: JsonPropertyName("lat"), Key("lat")] double Lat,
    [property: JsonPropertyName("lon"), Key("lon")] double Lon,
    [property: JsonPropertyName("title"), Key("title")] string Title,
    [property: JsonPropertyName("content"), Key("content")] string Content) : SegmentData;

[MessagePackObject]
public record SegmentReply([property: JsonPropertyName("message_id"), Key("message_id")] string MessageId,
    [property: JsonPropertyName("user_id"), Key("user_id")] string UserId) : SegmentData;
