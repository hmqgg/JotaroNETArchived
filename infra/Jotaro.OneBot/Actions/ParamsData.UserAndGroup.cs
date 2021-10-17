using System.Text.Json.Serialization;
using MessagePack;

namespace Jotaro.OneBot.Actions;

[MessagePackObject]
public abstract record ParamsGroupAndUser([property: JsonPropertyName("group_id"), Key("group_id")] string GroupId,
    [property: JsonPropertyName("user_id"), Key("user_id")] string UserId) : ParamsData;

[MessagePackObject]
public record ParamsGetSelfInfo : ParamsData;

[MessagePackObject]
public record ParamsGetUserInfo([property: JsonPropertyName("user_id"), Key("user_id")] string UserId) : ParamsData;

[MessagePackObject]
public record ParamsGetFriendList : ParamsData;

[MessagePackObject]
public record ParamsGetGroupInfo([property: JsonPropertyName("group_id"), Key("group_id")] string GroupId) : ParamsData;

[MessagePackObject]
public record ParamsGetGroupList : ParamsData;

[MessagePackObject]
public record ParamsGetGroupMemberInfo(string GroupId, string UserId) : ParamsGroupAndUser(GroupId, UserId);

[MessagePackObject]
public record ParamsGetGroupMemberList(string GroupId, string UserId) : ParamsGroupAndUser(GroupId, UserId);

[MessagePackObject]
public record ParamsSetGroupName([property: JsonPropertyName("group_id"), Key("group_id")] string GroupId,
    [property: JsonPropertyName("group_name"), Key("group_name")] string GroupName) : ParamsData;

[MessagePackObject]
public record ParamsLeaveGroup([property: JsonPropertyName("group_id"), Key("group_id")] string GroupId) : ParamsData;

[MessagePackObject]
public record ParamsKickGroupMember(string GroupId, string UserId) : ParamsGroupAndUser(GroupId, UserId);

[MessagePackObject]
public record ParamsBanGroupMember(string GroupId, string UserId) : ParamsGroupAndUser(GroupId, UserId);

[MessagePackObject]
public record ParamsUnbanGroupMember(string GroupId, string UserId) : ParamsGroupAndUser(GroupId, UserId);

[MessagePackObject]
public record ParamsSetGroupAdmin(string GroupId, string UserId) : ParamsGroupAndUser(GroupId, UserId);

[MessagePackObject]
public record ParamsUnsetGroupAdmin(string GroupId, string UserId) : ParamsGroupAndUser(GroupId, UserId);
