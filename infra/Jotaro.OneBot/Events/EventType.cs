using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Jotaro.OneBot.Events.Messages;
using Jotaro.OneBot.Events.Metas;
using Jotaro.OneBot.Events.Notices;
using Jotaro.OneBot.Events.Requests;

namespace Jotaro.OneBot.Events;

/// <summary>
///     EventType is no need to be extensible.
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum EventType
{
    [EnumMember(Value = EventMeta.EventTypeName)]
    Meta,

    [EnumMember(Value = EventMessage.EventTypeName)]
    Message,

    [EnumMember(Value = EventNotice.EventTypeName)]
    Notice,

    [EnumMember(Value = EventRequest.EventTypeName)]
    Request
}
