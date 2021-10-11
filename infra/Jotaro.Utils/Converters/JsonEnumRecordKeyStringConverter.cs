using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jotaro.Utils.Converters;

public sealed class JsonEnumRecordKeyStringConverter<TEnum, TKey> : JsonConverter<TEnum>
    where TEnum : EnumRecord<TEnum, TKey> where TKey : IEquatable<TKey>, IComparable<TKey>
{
    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var key = reader.TokenType switch
        {
            JsonTokenType.String => JsonSerializer.Deserialize<TKey>(ref reader),
            _ => throw new JsonException(
                $"Unexpected token {reader.TokenType} when parsing {nameof(EnumRecord<TEnum, TKey>)}.")
        };

        return EnumRecord<TEnum, TKey>.TryFromKey(key!, out var result) ? result : default;
    }

    /// <summary>
    ///     As is `TValue`.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value.Key, options);
}
