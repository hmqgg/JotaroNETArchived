using System.Text.Json.Serialization;
using MessagePack;

namespace Jotaro.OneBot.Actions;

public abstract record ParamsData
{
    [JsonExtensionData, IgnoreMember]
    public Dictionary<string, object> ExtensionData { get; init; } = new();
}

[MessagePackObject]
public record ParamsUnknown : ParamsData;
