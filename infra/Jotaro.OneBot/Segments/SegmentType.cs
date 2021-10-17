using System.Text.Json.Serialization;
using Jotaro.Utils;
using Jotaro.Utils.Converters;
using MessagePack;

namespace Jotaro.OneBot.Segments;

/// <summary>
///     Segment is extensible:
///     <see href="https://1bot.dev/specs/message/" />.
/// </summary>
[JsonConverter(typeof(JsonEnumRecordKeyStringConverter<SegmentType, string>)),
 MessagePackFormatter(typeof(MsgPackEnumRecordKeyStringFormatter<SegmentType, string>))]
public record SegmentType : EnumRecord<SegmentType, string>
{
    /// <summary>
    ///     For those Segments of which type is unknown, `Key` would be overwritten by the deserializer.
    /// </summary>
    public static readonly SegmentType Unknown = new(string.Empty, typeof(SegmentUnknown));

    // ReSharper disable once ConvertToPrimaryConstructor
    public SegmentType(string key, Type classType) : base(key) => ClassType = classType;

    // Remove `init` accessor here to prevent `with` access.
    public Type ClassType { get; }

    #region OneBot v12 Standard Message Segments

    public static readonly SegmentType Text = new("text", typeof(SegmentText));
    public static readonly SegmentType Image = new("image", typeof(SegmentImage));
    public static readonly SegmentType Voice = new("voice", typeof(SegmentVoice));
    public static readonly SegmentType File = new("file", typeof(SegmentFile));
    public static readonly SegmentType Mention = new("mention", typeof(SegmentMention));
    public static readonly SegmentType Video = new("video", typeof(SegmentVideo));
    public static readonly SegmentType Location = new("location", typeof(SegmentLocation));
    public static readonly SegmentType Reply = new("reply", typeof(SegmentReply));

    #endregion
}
