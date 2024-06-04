using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using SocialMediaService.Application.Features.Commands.CancelJoinRequest;
using SocialMediaService.Application.Features.Commands.ChangeMemberRole;
using SocialMediaService.Application.Features.Commands.CreateDiscussion;
using SocialMediaService.Application.Features.Commands.CreateGroup;
using SocialMediaService.Application.Features.Commands.DeleteDiscussion;
using SocialMediaService.Application.Features.Commands.DeleteGroup;
using SocialMediaService.Application.Features.Commands.DeleteMember;
using SocialMediaService.Application.Features.Commands.LeaveGroup;
using SocialMediaService.Application.Features.Commands.RespondToInvite;
using SocialMediaService.Application.Features.Commands.RespondToJoinRequest;
using SocialMediaService.Application.Features.Commands.SendInvite;
using SocialMediaService.Application.Features.Commands.SendJoinRequest;
using SocialMediaService.Application.Features.Commands.UpdateDiscussion;
using SocialMediaService.Application.Features.Commands.UpdateGroup;
using SocialMediaService.Application.Features.Queries.GetDiscussion;
using SocialMediaService.Application.Features.Queries.GetDiscussionsPage;
using SocialMediaService.Application.Features.Queries.GetGroup;
using SocialMediaService.Application.Features.Queries.GetGroupMembersPage;
using SocialMediaService.Application.Features.Queries.GetGroupsPage;
using SocialMediaService.Application.Features.Queries.GetJoinRequestsPage;
using SocialMediaService.Application.Features.Queries.GetKickedPage;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.WebApi.Dtos.GroupDtos;
using SocialMediaService.WebApi.Extensions;

namespace SocialMediaService.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest dto)
    {
        var result = await _mediator.Send(new CreateGroupCommand(User.GetId()!,
            dto.Name,
            dto.Description,
            dto.Visibility,
            dto.Image,
            dto.CoverImage));

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

        var result = await _mediator.Send(new GetGroupsPageQuery(pageRequest));

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
    [HttpPost("{id}/send-request")]
    public async Task<IActionResult> SendJoinRequest(string id, [FromBody] SendJoinRequestRequest dto)
    {
        var result = await _mediator.Send(new SendJoinRequestCommand(id, User.GetId()!, dto.Content));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/requests")]
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
    [HttpPost("{id}/send-invite/{profileId}")]
    public async Task<IActionResult> SendInvite(string id, string profileId, [FromBody] string content)
    {
        var result = await _mediator.Send(new SendInviteCommand(profileId, id, User.GetId()!, content));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/invites/{senderId}")]
    public async Task<IActionResult> SendInvite(string id, string senderId, [FromQuery] bool accept)
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
    public async Task<IActionResult> Kick(string id, string memberId, [FromBody] string reason)
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

    #region Discussion
    [HttpGet("{id}/discussions")]
    public async Task<IActionResult> Discussions(string id,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var page = new PageRequest<Discussion>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetDiscussionsPageQuery(id, page, User.GetId()!));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/discussions")]
    public async Task<IActionResult> CreateDiscussion(string id, [FromBody] CreateDiscussionRequest dto)
    {
        var result = await _mediator.Send(new CreateDiscussionCommand(User.GetId()!, id, dto.Title, dto.Content, dto.Tags));

        return this.GetFromResult(result);
    }

    [HttpGet("{id}/discussions/{discussionId}")]
    public async Task<IActionResult> Discussion(string id, string discussionId)
    {
        var result = await _mediator.Send(new GetDiscussionQuery(id, discussionId, User.GetId()!));

        return this.GetFromResult(result);
    }

    [HttpPatch("{id}/discussions/{discussionId}")]
    public async Task<IActionResult> UpdateDiscussion(string id, string discussionId, [FromBody] CreateDiscussionRequest dto)
    {
        var result = await _mediator.Send(new UpdateDiscussionCommand(id, discussionId, User.GetId()!, dto.Title, dto.Content, dto.Tags));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/discussions/{discussionId}")]
    public async Task<IActionResult> DeleteDiscussion(string id, string discussionId)
    {
        var result = await _mediator.Send(new DeleteDiscussionCommand(id, discussionId, User.GetId()!));

        return this.GetFromResult(result);
    }
    #endregion // Discussion
}