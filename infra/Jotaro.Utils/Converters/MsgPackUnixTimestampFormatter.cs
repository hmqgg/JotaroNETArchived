using MessagePack;
using MessagePack.Formatters;

namespace Jotaro.Utils.Converters;

public class MsgPackUnixTimestampFormatter : IMessagePackFormatter<DateTimeOffset>
{
    public void Serialize(ref MessagePackWriter writer, DateTimeOffset value, MessagePackSerializerOptions options) =>
        MessagePackSerializer.Serialize(ref writer, value.ToUnixTimeSeconds(), options);

    public DateTimeOffset Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        long time;

        // Check depth to avoid StackOverflow.
        options.Security.DepthStep(ref reader);
        try
        {
            time = MessagePackSerializer.Deserialize<long>(ref reader, options);
        }
        finally
        {
            reader.Depth--;
        }

        return DateTimeOffset.FromUnixTimeSeconds(time);
    }
}
