using MessagePack;
using MessagePack.Formatters;

namespace Jotaro.Utils.Converters;

public sealed class MsgPackEnumRecordKeyStringFormatter<TEnum, TKey> : IMessagePackFormatter<TEnum>
    where TEnum : EnumRecord<TEnum, TKey> where TKey : IEquatable<TKey>, IComparable<TKey>
{
    public void Serialize(ref MessagePackWriter writer, TEnum value, MessagePackSerializerOptions options) =>
        MessagePackSerializer.Serialize(ref writer, value.Key.ToString(), options);

    public TEnum Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        TKey key;

        // Check depth to avoid StackOverflow.
        options.Security.DepthStep(ref reader);
        try
        {
            key = MessagePackSerializer.Deserialize<TKey>(ref reader, options);
        }
        finally
        {
            reader.Depth--;
        }

        return EnumRecord<TEnum, TKey>.TryFromKey(key!, out var result) ? result! : null!;
    }
}
