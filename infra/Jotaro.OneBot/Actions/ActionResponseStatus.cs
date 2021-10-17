using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Jotaro.OneBot.Actions;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum ActionResponseStatus
{
    [EnumMember(Value = ConstantsOfAction.ResponseStatusFailedName)]
    Failed = 0,

    [EnumMember(Value = ConstantsOfAction.ResponseStatusOkName)]
    Ok = 1
}
