using Jotaro.OneBot.Events.Messages;
using Jotaro.OneBot.Events.Metas;
using Jotaro.OneBot.Events.Notices;
using Jotaro.OneBot.Events.Requests;
using Jotaro.Utils;

namespace Jotaro.OneBot.Events;

/// <summary>
///     EventSet is extensible:
///     <see href="https://1bot.dev/onebotrpc/data-protocol/event/" />.
///     <br />
///     List of Standard EventSets:
///     <see href="https://1bot.dev/specs/event/" />.
/// </summary>
public record EventSet : EnumRecord<EventSet, (EventType Type, string DetailType)>
{
    private EventSet((EventType Type, string DetailType) key, Type classType) : base(key) => ClassType = classType;

    public Type ClassType { get; }

    public EventType Type => Key.Type;

    public string DetailType => Key.DetailType;

    public static EventSet NewMessage(string detailType, Type classType) =>
        new((EventType.Message, detailType), classType);

    public static EventSet NewMeta(string detailType, Type classType) =>
        new((EventType.Meta, detailType), classType);

    public static EventSet NewNotice(string detailType, Type classType) =>
        new((EventType.Notice, detailType), classType);

    public static EventSet NewRequest(string detailType, Type classType) =>
        new((EventType.Request, detailType), classType);

    #region OneBot v12 Standard Events

    public static readonly EventSet Message = NewMessage(string.Empty, typeof(EventMessage));
    public static readonly EventSet MessagePrivate = NewMessage("private", typeof(EventMessagePrivate));
    public static readonly EventSet MessageGroup = NewMessage("group", typeof(EventMessageGroup));

    public static readonly EventSet Meta = NewMeta(string.Empty, typeof(EventMeta));
    public static readonly EventSet MetaHeartbeat = NewMeta("heartbeat", typeof(EventMetaHeartbeat));

    public static readonly EventSet Notice = NewNotice(string.Empty, typeof(EventNotice));
    public static readonly EventSet NoticeGroupIncrease = NewNotice("group_increase", typeof(EventNoticeGroupIncrease));
    public static readonly EventSet NoticeGroupDecrease = NewNotice("group_decrease", typeof(EventNoticeGroupDecrease));
    public static readonly EventSet NoticeGroupAdmin = NewNotice("group_admin", typeof(EventNoticeGroupAdmin));
    public static readonly EventSet NoticeGroupBan = NewNotice("group_ban", typeof(EventNoticeGroupBan));
    public static readonly EventSet NoticeFriend = NewNotice("friend", typeof(EventNoticeFriend));

    public static readonly EventSet NoticeGroupMessageDelete =
        NewNotice("group_message_delete", typeof(EventNoticeGroupMessageDelete));

    public static readonly EventSet NoticePrivateMessageDelete =
        NewNotice("private_message_delete", typeof(EventNoticePrivateMessageDelete));

    public static readonly EventSet Request = NewRequest(string.Empty, typeof(EventRequest));

    #endregion
}
