using System.Text.Json.Serialization;
using MessagePack;
using MessagePack.Formatters;

namespace Jotaro.OneBot.Actions;

[MessagePackObject]
public sealed record ActionResponse<TRespData> where TRespData : class
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public ActionResponse(TRespData data, ActionResponseStatus status = ActionResponseStatus.Ok, long retCode = 0,
        string message = "", string echo = "")
    {
        Status = status;
        RetCode = retCode;
        Data = data;
        Message = message;
        Echo = echo;
    }

    [JsonPropertyName(ConstantsOfAction.StatusPropertyName), Key(ConstantsOfAction.StatusPropertyName),
     JsonConverter(typeof(JsonStringEnumMemberConverter)),
     MessagePackFormatter(typeof(EnumAsStringFormatter<ActionResponseStatus>))]
    public ActionResponseStatus Status { get; } = ActionResponseStatus.Ok;

    [JsonPropertyName(ConstantsOfAction.RetCodePropertyName), Key(ConstantsOfAction.RetCodePropertyName)]
    public long RetCode { get; }

    [JsonPropertyName(ConstantsOfAction.DataPropertyName), Key(ConstantsOfAction.DataPropertyName)]
    public TRespData Data { get; }

    [JsonPropertyName(ConstantsOfAction.MessagePropertyName), Key(ConstantsOfAction.MessagePropertyName)]
    public string Message { get; } = string.Empty;

    [JsonPropertyName(ConstantsOfAction.EchoPropertyName), Key(ConstantsOfAction.EchoPropertyName)]
    public string Echo { get; } = string.Empty;
}
