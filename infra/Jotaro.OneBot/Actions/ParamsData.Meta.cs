using System.Text.Json.Serialization;
using MessagePack;

namespace Jotaro.OneBot.Actions;

[MessagePackObject]
public record ParamsGetLatestEvents([property: JsonPropertyName("limit"), Key("limit")] long Limit = 0,
    [property: JsonPropertyName("timeout"), Key("timeout")] long Timeout = 0) : ParamsData;

[MessagePackObject]
public record ParamsGetSupportedActions : ParamsData;

[MessagePackObject]
public record ParamsGetStatus : ParamsData;

[MessagePackObject]
public record ParamsGetVersion : ParamsData;
