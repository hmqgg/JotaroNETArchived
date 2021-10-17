using System.Text.Json.Serialization;
using MessagePack;

namespace Jotaro.OneBot.Actions;

[MessagePackObject]
public record ParamsUploadFile([property: JsonPropertyName("type"), Key("type")] string Type,
    [property: JsonPropertyName("name"), Key("name")] string Name,
    [property: JsonPropertyName("url"), Key("url")] string Url,
    [property: JsonPropertyName("headers"), Key("headers")] IDictionary<string, string> Headers,
    [property: JsonPropertyName("path"), Key("path")] string Path,
    [property: JsonPropertyName("data"), Key("data")] byte[] Data,
    [property: JsonPropertyName("sha256"), Key("sha256")] string Sha256) : ParamsData;

[MessagePackObject]
public record ParamsUploadFileByUrl
    (string Name, string Url, IDictionary<string, string> Headers, string Sha256 = "") : ParamsUploadFile("url", Name,
        Url, Headers, string.Empty, Array.Empty<byte>(), Sha256);

[MessagePackObject]
public record ParamsUploadFileByPath(string Name, string Path, string Sha256 = "") : ParamsUploadFile("path", Name,
    string.Empty, new Dictionary<string, string>(), Path, Array.Empty<byte>(), Sha256);

[MessagePackObject]
public record ParamsUploadFileByData(string Name, byte[] Data, string Sha256 = "") : ParamsUploadFile("data", Name,
    string.Empty, new Dictionary<string, string>(), string.Empty, Data, Sha256);

[MessagePackObject]
public record ParamsUploadFileFragmented([property: JsonPropertyName("stage"), Key("stage")] string Stage,
    [property: JsonPropertyName("name"), Key("name")] string Name,
    [property: JsonPropertyName("total_size"), Key("total_size")] long TotalSize,
    [property: JsonPropertyName("size"), Key("size")] long Size,
    [property: JsonPropertyName("offset"), Key("offset")] long Offset,
    [property: JsonPropertyName("data"), Key("data")] byte[] Data,
    [property: JsonPropertyName("file_id"), Key("file_id")] string FileId,
    [property: JsonPropertyName("sha256"), Key("sha256")] string Sha256) : ParamsData;

[MessagePackObject]
public record ParamsUploadFileFragmentedPrepare(string Name, long TotalSize, string Sha256 = "") :
    ParamsUploadFileFragmented("prepare", Name, TotalSize, 0, 0, Array.Empty<byte>(), string.Empty, Sha256);

[MessagePackObject]
public record ParamsUploadFileFragmentedTransfer
    (string FileId, long Size, long Offset, byte[] Data) : ParamsUploadFileFragmented("transfer", string.Empty, 0, Size,
        Offset, Data, FileId, string.Empty);

[MessagePackObject]
public record ParamsUploadFileFragmentedFinish(string FileId) : ParamsUploadFileFragmented("finish", string.Empty, 0, 0,
    0, Array.Empty<byte>(), FileId, string.Empty);

[MessagePackObject]
public record ParamsGetFile([property: JsonPropertyName("type"), Key("type")] string Type,
    [property: JsonPropertyName("file_id"), Key("file_id")] string FileId) : ParamsData;

[MessagePackObject]
public record ParamsGetFileByUrl(string FileId) : ParamsGetFile("url", FileId);

[MessagePackObject]
public record ParamsGetFileByPath(string FileId) : ParamsGetFile("path", FileId);

[MessagePackObject]
public record ParamsGetFileByData(string FileId) : ParamsGetFile("data", FileId);

[MessagePackObject]
public record ParamsGetFileFragmented([property: JsonPropertyName("stage"), Key("stage")] string Stage,
    [property: JsonPropertyName("file_id"), Key("file_id")] string FileId,
    [property: JsonPropertyName("size"), Key("size")] long Size,
    [property: JsonPropertyName("offset"), Key("offset")] long Offset) : ParamsData;

[MessagePackObject]
public record ParamsGetFileFragmentedPrepare(string FileId) : ParamsGetFileFragmented("prepare", FileId, 0, 0);

[MessagePackObject]
public record ParamsGetFileFragmentedTransfer(string FileId, long Size, long Offset) : ParamsGetFileFragmented(
    "transfer", FileId, Size, Offset);
