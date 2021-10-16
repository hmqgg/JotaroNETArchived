using System.Text.Json.Serialization;
using MessagePack;

namespace Jotaro.OneBot.Segments;

[JsonConverter(typeof(JsonSegmentConverter)), MessagePackFormatter(typeof(MsgPackSegmentFormatter))]
public sealed record Segment
{
    internal const string TypePropertyName = "type";
    internal const string DataPropertyName = "data";

    private static readonly Dictionary<Type, SegmentType> sTypeCache = new();

    internal Segment(SegmentType type, SegmentData data)
    {
        Type = type;
        Data = data;
    }

    public Segment(SegmentData data)
    {
        var dataClassType = data.GetType();

        if (!sTypeCache.TryGetValue(dataClassType, out var type))
        {
            // Do not use `IsAssignableFrom` here.
            type = SegmentType.FirstOrDefault(t => t.ClassType == dataClassType);
            if (type is null)
            {
                throw new NotSupportedException($"{nameof(Type)} {dataClassType.Name} is not supported.");
            }

            // Cache it.
            sTypeCache.Add(dataClassType, type);
        }

        Type = type;
        Data = data;
    }

    [JsonPropertyName(TypePropertyName), Key(TypePropertyName)]
    public SegmentType Type { get; }

    [JsonPropertyName(DataPropertyName), Key(DataPropertyName)]
    public SegmentData Data { get; }

    public static implicit operator SegmentData(Segment s) => s.Data;
    public static implicit operator Segment(SegmentData d) => new(d);
}
