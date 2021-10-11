using System.Buffers;
using Cysharp.Text;
using gfoidl.Base64;

namespace Jotaro.Utils.Helpers;

public static class StreamBase64EncodeExtensions
{
    public static ReadOnlySpan<char> ToBase64String(this Stream stream)
    {
        if (!stream.CanRead)
        {
            return string.Empty;
        }

        using var zsb = ZString.CreateStringBuilder();
        using var owner = MemoryPool<byte>.Shared.Rent();
        var offset = 0;

        while (true)
        {
            var read = stream.Read(owner.Memory.Span[offset..]);

            if (read == 0)
            {
                // Read to end.
                if (offset > 0)
                {
                    // To ensure there is no padding in the middle of encoded base64.
                    var lastLen = Base64.Default.GetEncodedLength(offset);
                    Base64.Default.Encode(owner.Memory.Span[..offset], zsb.GetSpan(lastLen), out _, out var lastW);
                    zsb.Advance(lastW);
                }

                break;
            }

            var encodedLen = Base64.Default.GetEncodedLength(offset + read);
            Base64.Default.Encode(owner.Memory.Span[..(offset + read)], zsb.GetSpan(encodedLen), out var consumed,
                out var written, false);
            zsb.Advance(written);

            offset += read - consumed;
            if (offset > 0)
            {
                owner.Memory[consumed..].CopyTo(owner.Memory);
            }
        }

        return zsb.ToString();
    }
}
