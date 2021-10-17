using System.Text;
using MessagePack;

namespace Jotaro.OneBot.Actions;

internal static class ConstantsOfAction
{
    internal const string ActionPropertyName = "action";
    internal const string ParamsPropertyName = "params";
    internal const string EchoPropertyName = "echo";

    internal const string StatusPropertyName = "status";
    internal const string RetCodePropertyName = "retcode";
    internal const string DataPropertyName = "data";
    internal const string MessagePropertyName = "message";

    internal const string ResponseStatusOkName = "ok";
    internal const string ResponseStatusFailedName = "failed";

    internal static readonly byte[] sActionPropertyName = Encoding.UTF8.GetBytes(ActionPropertyName);
    internal static readonly byte[] sParamsPropertyName = Encoding.UTF8.GetBytes(ParamsPropertyName);
    internal static readonly byte[] sEchoPropertyName = Encoding.UTF8.GetBytes(EchoPropertyName);

    internal static readonly byte[] sEmptyMsgPack = MessagePackSerializer.ConvertFromJson("{}");
}
