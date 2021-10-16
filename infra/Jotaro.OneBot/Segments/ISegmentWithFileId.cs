using System.Text.Json.Serialization;
using MessagePack;

namespace Jotaro.OneBot.Segments;

public interface ISegmentWithFileId
{
    [JsonPropertyName("file_id"), Key("file_id")]
    public string FileId { get; init; }
}
