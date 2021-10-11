namespace Jotaro.OneBot.Events.Requests;

public abstract record EventRequest
    (string SubType, string Uuid, string SelfId, string Platform, DateTimeOffset Time) : Event(EventSet.Request,
        SubType, Uuid, SelfId, Platform, Time)
{
    internal const string EventTypeName = "request";
}
