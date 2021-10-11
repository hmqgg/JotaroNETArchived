using System.Buffers;
using System.Text;
using MessagePack;
using MessagePack.Formatters;

namespace Jotaro.OneBot.Segments;

public sealed class MsgPackSegmentFormatter : IMessagePackFormatter<Segment>
{
    private static readonly byte[] sTypePropertyName = Encoding.UTF8.GetBytes(Segment.TypePropertyName);
    private static readonly byte[] sDataPropertyName = Encoding.UTF8.GetBytes(Segment.DataPropertyName);

    public void Serialize(ref MessagePackWriter writer, Segment value, MessagePackSerializerOptions options)
    {
        // 2-element map, hard-coded.
        writer.WriteMapHeader(2);

        // 4-byte string `type`.
        writer.WriteString(sTypePropertyName);
        // n-byte string e.g. `text` for `SegmentText`.
        MessagePackSerializer.Serialize(ref writer, value.Type, options);

        // 4-byte string `data`.
        writer.WriteString(sDataPropertyName);
        // Serialize `SegmentData`.
        // BUG: `SegmentData` ignores `ExtensionData`.
        MessagePackSerializer.Serialize(value.Type.ClassType, ref writer, value.Data, options);
    }

    public Segment Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (!reader.TryReadMapHeader(out var elementCount))
        {
            throw new MessagePackSerializationException();
        }

        var typeSeq = ReadOnlySequence<byte>.Empty;
        var dataSeq = ReadOnlySequence<byte>.Empty;

        // `elementCount` shall be 2.
        for (var i = 0; i < elementCount; i++)
        {
            if (reader.TryReadStringSpan(out var propertyName))
            {
                if (propertyName.SequenceEqual(sTypePropertyName))
                {
                    typeSeq = reader.ReadRaw();
                }
                else if (propertyName.SequenceEqual(sDataPropertyName))
                {
                    dataSeq = reader.ReadRaw();
                }
            }
        }

        var type = MessagePackSerializer.Deserialize<SegmentType>(typeSeq, options) ?? SegmentType.Unknown with
        {
            Key = MessagePackSerializer.Deserialize<string>(typeSeq, options)
        };
        var data = MessagePackSerializer.Deserialize(type.ClassType, dataSeq, options);

        if (data is SegmentData d)
        {
            return new Segment(type, d);
        }

        // Failed in deserialize `data`, not supported `Segment` yet.
        throw new NotSupportedException(
            $"{nameof(SegmentType)} {Encoding.UTF8.GetString(typeSeq)} is not supported by {nameof(MsgPackSegmentFormatter)}.");
    }
}
