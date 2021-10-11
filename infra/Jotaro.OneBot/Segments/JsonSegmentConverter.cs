using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jotaro.OneBot.Segments;

public sealed class JsonSegmentConverter : JsonConverter<Segment>
{
    private static readonly byte[] sTypePropertyName = Encoding.UTF8.GetBytes(Segment.TypePropertyName);
    private static readonly byte[] sDataPropertyName = Encoding.UTF8.GetBytes(Segment.DataPropertyName);

    public override Segment Read(ref Utf8JsonReader reader, Type _, JsonSerializerOptions options)
    {
        if (JsonDocument.TryParseValue(ref reader, out var doc) &&
            doc.RootElement.TryGetProperty(sTypePropertyName, out var typeProperty) &&
            doc.RootElement.TryGetProperty(sDataPropertyName, out var dataProperty))
        {
            // Fallback to `SegmentUnknown` and overwrite `Segment.Type`.
            var type = typeProperty.Deserialize<SegmentType>(options) ?? SegmentType.Unknown with
            {
                // Call `GetString()` instead of `GetRawText()` to prevent redundant quotes.
                Key = typeProperty.GetString() ?? string.Empty
            };
            var data = dataProperty.Deserialize(type.ClassType, options);

            if (data is SegmentData d)
            {
                return new Segment(type, d);
            }

            // Failed in deserialize `data`, not supported `Segment` yet.
            throw new NotSupportedException(
                $"{nameof(SegmentType)} {typeProperty.GetRawText()} is not supported by {nameof(JsonSegmentConverter)}.");
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Segment value, JsonSerializerOptions options)
    {
        // {
        writer.WriteStartObject();

        // "type":"text",
        writer.WritePropertyName(sTypePropertyName);
        JsonSerializer.Serialize(writer, value.Type, options);

        // "data":{"text":"Hello, world!"}
        writer.WritePropertyName(sDataPropertyName);
        JsonSerializer.Serialize(writer, value.Data, value.Type.ClassType, options);

        // }
        writer.WriteEndObject();
    }
}
