using PR2.Shared.Enums;

namespace SocialMediaService.WebApi.Dtos.GroupDtos;

public record UpdateGroupSettingsRequest(MemberRoleTypes? InviterRole = null,
    MemberRoleTypes? PostingRole = null,
    MemberRoleTypes? EditDetailsRole = null);