using System.Text.Json.Serialization;
using Jotaro.Utils;
using Jotaro.Utils.Converters;
using MessagePack;

namespace Jotaro.OneBot.Actions;

/// <summary>
///     ActionType is extensible:
///     <see href="https://1bot.dev/onebotrpc/data-protocol/action-request/" />.
///     <br />
///     List of Standard ActionTypes:
///     <see href="https://1bot.dev/specs/action/" />.
/// </summary>
[JsonConverter(typeof(JsonEnumRecordKeyStringConverter<ActionType, string>)),
 MessagePackFormatter(typeof(MsgPackEnumRecordKeyStringFormatter<ActionType, string>))]
public record ActionType : EnumRecord<ActionType, string>
{
    /// <summary>
    ///     For those Segments of which type is unknown, `Key` would be overwritten by the deserializer.
    /// </summary>
    public static readonly ActionType Unknown = new(string.Empty, typeof(ActionRequestUnknown));

    #region OneBot v12 Standard Actions

    #region Meta Action

    #endregion

    #region Message Action

    public static readonly ActionType SendMessage = new("send_message", typeof(ActionRequestSendMessage));

    #endregion

    #endregion

    // ReSharper disable once ConvertToPrimaryConstructor
    public ActionType(string key, Type classType) : base(key) => ClassType = classType;

    // Remove `init` accessor here to prevent `with` access.
    public Type ClassType { get; }
}
