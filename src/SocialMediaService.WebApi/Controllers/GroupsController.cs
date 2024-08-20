using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using SocialMediaService.Application.Features.Commands.CancelJoinRequest;
using SocialMediaService.Application.Features.Commands.ChangeMemberRole;
using SocialMediaService.Application.Features.Commands.CreateGroup;
using SocialMediaService.Application.Features.Commands.DeleteGroup;
using SocialMediaService.Application.Features.Commands.DeleteMember;
using SocialMediaService.Application.Features.Commands.LeaveGroup;
using SocialMediaService.Application.Features.Commands.RespondToInvite;
using SocialMediaService.Application.Features.Commands.RespondToJoinRequest;
using SocialMediaService.Application.Features.Commands.SendInvite;
using SocialMediaService.Application.Features.Commands.SendJoinRequest;
using SocialMediaService.Application.Features.Commands.UpdateGroup;
using SocialMediaService.Application.Features.Queries.GetGroup;
using SocialMediaService.Application.Features.Queries.GetGroupMembersPage;
using SocialMediaService.Application.Features.Queries.GetGroupPostsPage;
using SocialMediaService.Application.Features.Queries.GetGroupsNamesByIds;
using SocialMediaService.Application.Features.Queries.GetGroupsPage;
using SocialMediaService.Application.Features.Queries.GetJoinRequestsPage;
using SocialMediaService.Application.Features.Queries.GetKickedPage;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.WebApi.Dtos.GroupDtos;
using SocialMediaService.WebApi.Extensions;

namespace SocialMediaService.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ids")]
    public async Task<IActionResult> GetNamesByIds([FromHeader] IEnumerable<string> ids)
    {
        var result = await _mediator.Send(new GetGroupsNamesByIdsQuery(ids));

        return this.GetFromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest dto)
    {
        var result = await _mediator.Send(new CreateGroupCommand(User.GetId()!,
            dto.Name,
            dto.Description,
            dto.Visibility));

        return this.GetFromResult(result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateRequest dto)
    {
        var result = await _mediator.Send(new UpdateGroupCommand(User.GetId()!,
            id,
            dto.Name,
            dto.Description,
            dto.Visibility));

        return this.GetFromResult(result);
    }

    [HttpPatch("{id}/settings")]
    public async Task<IActionResult> UpdateSettings(string id, [FromBody] UpdateGroupSettingsRequest dto)
    {
        var result = await _mediator.Send(new UpdateGroupCommand(User.GetId()!,
            id,
            InviterRole: dto.InviterRole,
            PostingRole: dto.PostingRole,
            EditDetailsRole: dto.EditDetailsRole));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteGroupCommand(id, User.GetId()!));

        return this.GetFromResult(result);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Page([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Group>(pageNumber,
            pageSize,
            x => x.Name.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetGroupsPageQuery(search, pageRequest));

        return this.GetFromResult(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> Single(string id)
    {
        var result = await _mediator.Send(new GetGroupQuery(id, User.GetId()));

        return this.GetFromResult(result);
    }
    
    [AllowAnonymous]
    [HttpGet("{id}/posts")]
    public async Task<IActionResult> Posts(string id,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
       var pageRequest = new PageRequest<Post>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetGroupPostsPageQuery(id, pageRequest, User.GetId()));

        return this.GetFromResult(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}/members")]
    public async Task<IActionResult> Members(string id,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Member>(pageNumber,
            pageSize,
            x => x.Profile.FirstName.Contains(search) || x.Profile.LastName.Contains(search),
            x => x.JointAtUtc,
            desc);

        var result = await _mediator.Send(new GetGroupMembersPageQuery(id, User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    #region Join Request
    [HttpPost("{id}/request")]
    public async Task<IActionResult> SendJoinRequest(string id, [FromBody] SendJoinRequestRequest dto)
    {
        var result = await _mediator.Send(new SendJoinRequestCommand(id, User.GetId()!, dto.Content));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/request")]
    public async Task<IActionResult> CancelJoinRequest(string id)
    {
        var result = await _mediator.Send(new CancelJoinRequestCommand(id, User.GetId()!));

        return this.GetFromResult(result);
    }

    [HttpGet("{id}/requests")]
    public async Task<IActionResult> JoinRequests([FromRoute(Name = "id")] string groupId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<JoinRequest>(pageNumber,
            pageSize,
            x => x.Profile.FirstName.Contains(search) || x.Profile.LastName.Contains(search),
            x => x.SentAtUtc,
            desc);

        var result = await _mediator.Send(new GetJoinRequestsPageQuery(groupId, User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpPatch("{id}/requests/{requesterId}")]
    public async Task<IActionResult> RespondToRequest(string id, string requesterId, [FromQuery] bool accept = true)
    {
        var result = await _mediator.Send(new RespondToJoinRequestCommand(id, requesterId, User.GetId()!, accept));

        return this.GetFromResult(result);
    }
    #endregion // Join Request

    #region Invite
    [HttpPost("{id}/invite/{profileId}")]
    public async Task<IActionResult> SendInvite(string id, string profileId, [FromBody] SendInviteRequest dto)
    {
        var result = await _mediator.Send(new SendInviteCommand(profileId, id, User.GetId()!, dto.Content));

        return this.GetFromResult(result);
    }

    [HttpPatch("{id}/invites/{senderId}")]
    public async Task<IActionResult> RespondToInvite(string id, string senderId, [FromQuery] bool accept)
    {
        var result = await _mediator.Send(new RespondToInviteCommand(User.GetId()!, id, senderId, accept));

        return this.GetFromResult(result);
    }
    #endregion // Invite

    #region Kicked
    [HttpGet("{id}/kicked")]
    public async Task<IActionResult> Kicked(string id,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var page = new PageRequest<Kicked>(pageNumber,
            pageSize,
            x => x.Profile.FirstName.Contains(search) || x.Profile.LastName.Contains(search),
            x => x.KickedAtUtc,
            desc);

        var result = await _mediator.Send(new GetKickedPageQuery(id, User.GetId()!, page));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/kick/{memberId}")]
    public async Task<IActionResult> Kick(string id, string memberId, [FromHeader] string reason)
    {
        var result = await _mediator.Send(new DeleteMemberCommand(memberId, id, User.GetId()!, true, reason));

        return this.GetFromResult(result);
    }
    #endregion // Kicked

    [HttpDelete("{id}/leave")]
    public async Task<IActionResult> LeaveGroup(string id)
    {
        var result = await _mediator.Send(new LeaveGroupCommand(User.GetId()!, id));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/remove/{memberId}")]
    public async Task<IActionResult> DeleteMember(string id, string memberId)
    {
        var result = await _mediator.Send(new DeleteMemberCommand(memberId, id, User.GetId()!));

        return this.GetFromResult(result);
    }

    [HttpPatch("{id}/change-role/{memberId}")]
    public async Task<IActionResult> ChangeRole(string id, string memberId, [FromQuery] MemberRoleTypes role)
    {
        var result = await _mediator.Send(new ChangeMemberRoleCommand(memberId, id, User.GetId()!, role));

        return this.GetFromResult(result);
    }
}