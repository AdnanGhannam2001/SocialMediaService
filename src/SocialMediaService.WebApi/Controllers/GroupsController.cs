using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR2.Shared.Common;
using SocialMediaService.Application.Features.Commands.CancelJoinRequest;
using SocialMediaService.Application.Features.Commands.CreateGroup;
using SocialMediaService.Application.Features.Commands.DeleteGroup;
using SocialMediaService.Application.Features.Commands.RespondToJoinRequest;
using SocialMediaService.Application.Features.Commands.SendJoinRequest;
using SocialMediaService.Application.Features.Commands.UpdateGroup;
using SocialMediaService.Application.Features.Queries.GetGroup;
using SocialMediaService.Application.Features.Queries.GetGroupMembersPage;
using SocialMediaService.Application.Features.Queries.GetGroupsPage;
using SocialMediaService.Application.Features.Queries.GetJoinRequestsPage;
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
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<Group>(pageNumber,
            pageSize,
            x => x.Name.Contains(search),
            x => x.CreatedAtUtc);

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
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<Member>(pageNumber,
            pageSize,
            x => x.Profile.FirstName.Contains(search) || x.Profile.LastName.Contains(search),
            x => x.JointAtUtc);

        var result = await _mediator.Send(new GetGroupMembersPageQuery(id, User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}")]
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
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<JoinRequest>(pageNumber,
            pageSize,
            x => x.Profile.FirstName.Contains(search) || x.Profile.LastName.Contains(search),
            x => x.SentAtUtc);

        var result = await _mediator.Send(new GetJoinRequestsPageQuery(groupId, User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpPatch("{id}/requests/{requesterId}")]
    public async Task<IActionResult> RespondToRequest(string id, string requesterId, [FromQuery] bool accept = true)
    {
        var result = await _mediator.Send(new RespondToJoinRequestCommand(id, requesterId, User.GetId()!, accept));

        return this.GetFromResult(result);
    }
}