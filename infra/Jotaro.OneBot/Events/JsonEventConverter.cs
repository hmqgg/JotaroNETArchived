using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jotaro.OneBot.Events;

public sealed class JsonEventConverter : JsonConverter<Event>
{
    private static readonly byte[] sTypePropertyName = Encoding.UTF8.GetBytes(Event.TypePropertyName);
    private static readonly byte[] sDetailTypePropertyName = Encoding.UTF8.GetBytes(Event.DetailTypePropertyName);

    public override Event Read(ref Utf8JsonReader reader, Type _, JsonSerializerOptions options)
    {
        if (JsonDocument.TryParseValue(ref reader, out var doc) &&
            doc.RootElement.TryGetProperty(sTypePropertyName, out var typeProperty) &&
            doc.RootElement.TryGetProperty(sDetailTypePropertyName, out var detailTypeProperty))
        {
            var type = typeProperty.Deserialize<EventType>(options);
            var detailType = detailTypeProperty.GetString() ?? string.Empty;

            // Try to parse EventSet.
            if (!EventSet.TryFromKey((type, detailType), out var set))
            {
                // Handle unknown `detail_type` events, but the `type` must be one of the four.
                set = type switch
                {
                    EventType.Meta => EventSet.NewMeta(detailType, typeof(EventMeta)),
                    EventType.Message => EventSet.NewMessage(detailType, typeof(EventMessage)),
                    EventType.Notice => EventSet.NewNotice(detailType, typeof(EventNotice)),
                    EventType.Request => EventSet.NewRequest(detailType, typeof(EventRequest)),
                    // `type` is unknown, not supported yet.
                    _ => throw new NotSupportedException(
                        $"{nameof(EventType)} {typeProperty.GetRawText()} is not supported by {nameof(JsonEventConverter)}.")
                };
            }

            var post = doc.Deserialize(set!.ClassType, options);

            if (post is Event ev)
            {
                return ev;
            }
        }

        throw new JsonException();
    }

    /// <summary>
    ///     As is. Self-reference/StackOverflowEx would happen if without <c>value.GetType()</c>.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Event value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
