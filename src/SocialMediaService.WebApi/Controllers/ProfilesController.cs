using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR2.Shared.Common;
using SocialMediaService.Application.Features.Commands.AddToBlockList;
using SocialMediaService.Application.Features.Commands.CancelFriendship;
using SocialMediaService.Application.Features.Commands.CancelFriendshipRequest;
using SocialMediaService.Application.Features.Commands.DeleteFromBlockedList;
using SocialMediaService.Application.Features.Commands.FollowAccount;
using SocialMediaService.Application.Features.Commands.RespondToFriendshipRequest;
using SocialMediaService.Application.Features.Commands.SavePost;
using SocialMediaService.Application.Features.Commands.SendFriendshipRequest;
using SocialMediaService.Application.Features.Commands.UnfollowAccount;
using SocialMediaService.Application.Features.Commands.UnsavePost;
using SocialMediaService.Application.Features.Commands.UpdateProfile;
using SocialMediaService.Application.Features.Commands.UpdateSettings;
using SocialMediaService.Application.Features.Queries.GetBlockedPage;
using SocialMediaService.Application.Features.Queries.GetFollowedPage;
using SocialMediaService.Application.Features.Queries.GetFollowingPage;
using SocialMediaService.Application.Features.Queries.GetFriendshipRequestsPage;
using SocialMediaService.Application.Features.Queries.GetFriendshipsPage;
using SocialMediaService.Application.Features.Queries.GetGroupsPageFor;
using SocialMediaService.Application.Features.Queries.GetInvites;
using SocialMediaService.Application.Features.Queries.GetProfile;
using SocialMediaService.Application.Features.Queries.GetProfilesNamesByIds;
using SocialMediaService.Application.Features.Queries.GetProfilesPage;
using SocialMediaService.Application.Features.Queries.GetSettings;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.WebApi.Dtos.ProfileDtos;
using SocialMediaService.WebApi.Extensions;

namespace SocialMediaService.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Profile
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Profile>(pageNumber,
            pageSize,
            x => x.FirstName.Contains(search) || x.LastName.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetProfilesPageQuery(User.GetId(), pageRequest));

        return this.GetFromResult(result);
    }

    [HttpGet("ids")]
    public async Task<IActionResult> GetNamesByIds([FromHeader] IEnumerable<string> ids)
    {
        var result = await _mediator.Send(new GetProfilesNamesByIdsQuery(ids));

        return this.GetFromResult(result);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var result = await _mediator.Send(new GetProfileQuery(User.GetId()!));

        return this.GetFromResult(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetProfileQuery(id, true, User.GetId()));

        return this.GetFromResult(result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest request)
    {
        var result = await _mediator.Send(new UpdateProfileCommand(User.GetId()!,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.PhoneNumber,
            request.Bio,
            null, null,
            request.JobInformations,
            request.Socials));

        return this.GetFromResult(result);
    }
    #endregion // Profile

    #region Settings
    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
    {
        var result = await _mediator.Send(new GetSettingsQuery(User.GetId()!));

        return this.GetFromResult(result);
    }

    [HttpPatch("settings")]
    public async Task<IActionResult> UpdateSettings(UpdateProfileSettingsRequest request)
    {
        var result = await _mediator.Send(new UpdateSettingsCommand(User.GetId()!,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.Phone,
            request.JobTitle,
            request.Company,
            request.StartDate,
            request.Socials,
            request.Bio));

        return this.GetFromResult(result);
    }
    #endregion // Settings

    #region Block
    [HttpGet("blocked")]
    public async Task<IActionResult> Blocked([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Block>(pageNumber,
            pageSize,
            x => x.Blocked.FirstName.Contains(search) || x.Blocked.LastName.Contains(search),
            x => x.BlockedAtUtc,
            desc);

        var result = await _mediator.Send(new GetBlockedPageQuery(User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/block")]
    public async Task<IActionResult> Block([FromRoute(Name = "id")] string profileId, [FromBody] BlockRequest dto)
    {
        var result = await _mediator.Send(new AddToBlockListCommand(User.GetId()!, profileId, dto.Reason));

        return this.GetFromResult(result, StatusCodes.Status201Created);
    }

    [HttpDelete("{id}/block")]
    public async Task<IActionResult> Unblock([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new DeleteFromBlockedListCommand(User.GetId()!, profileId));

        return this.GetFromResult(result);
    }
    #endregion // Block

    #region Follow
    [HttpGet("followed")]
    public async Task<IActionResult> Followed([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Follow>(pageNumber,
            pageSize,
            x => x.Followed.FirstName.Contains(search) || x.Followed.LastName.Contains(search),
            x => x.FollowedAtUtc,
            desc);

        var result = await _mediator.Send(new GetFollowedPageQuery(User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpGet("{id}/following")]
    [AllowAnonymous]
    public async Task<IActionResult> Following([FromRoute] string id,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Follow>(pageNumber,
            pageSize,
            x => x.Follower.FirstName.Contains(search) || x.Follower.LastName.Contains(search),
            x => x.FollowedAtUtc,
            desc);

        var result = await _mediator.Send(new GetFollowingPageQuery(id, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/follow")]
    public async Task<IActionResult> Follow([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new FollowAccountCommand(User.GetId()!, profileId));

        return this.GetFromResult(result, StatusCodes.Status201Created);
    }

    [HttpDelete("{id}/follow")]
    public async Task<IActionResult> Unfollow([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new UnfollowAccountCommand(User.GetId()!, profileId));

        return this.GetFromResult(result);
    }
    #endregion // Follow

    #region Friendship Request
    [HttpGet("sent")]
    public async Task<IActionResult> SentFriendshipRequests([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<FriendshipRequest>(pageNumber,
            pageSize,
            x => x.Receiver.FirstName.Contains(search) || x.Receiver.LastName.Contains(search),
            x => x.SentAtUtc,
            desc);

        var result = await _mediator.Send(new GetFriendshipRequestsPageQuery(User.GetId()!, true, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpGet("received")]
    public async Task<IActionResult> ReceivedFriendshipRequests([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<FriendshipRequest>(pageNumber,
            pageSize,
            x => x.Sender.FirstName.Contains(search) || x.Sender.LastName.Contains(search),
            x => x.SentAtUtc,
            desc);

        var result = await _mediator.Send(new GetFriendshipRequestsPageQuery(User.GetId()!, false, pageRequest));

        return this.GetFromResult(result);
    }
    
    [HttpPost("{id}/request")]
    public async Task<IActionResult> SendFriendshipRequest([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new SendFriendshipRequestCommand(User.GetId()!, profileId));

        return this.GetFromResult(result, StatusCodes.Status201Created);
    }

    [HttpDelete("{id}/request")]
    public async Task<IActionResult> CancelFriendshipRequest([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new CancelFriendshipRequestCommand(User.GetId()!, profileId));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/respond")]
    public async Task<IActionResult> Respond([FromRoute(Name = "id")] string profileId, [FromQuery] bool accept = true)
    {
        var result = await _mediator.Send(new RespondToFriendshipRequestCommand(profileId, User.GetId()!, accept));

        return this.GetFromResult(result, 204);
    }
    #endregion // Friendship Request

    #region Friendship
    [HttpGet("friends")]
    public async Task<IActionResult> Friends([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Friendship>(pageNumber,
            pageSize,
            x => x.Friend.FirstName.Contains(search) || x.Friend.LastName.Contains(search),
            x => x.StartedAtUtc,
            desc);

        var result = await _mediator.Send(new GetFriendshipsPageQuery(User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpGet("{id}/friends")]
    [AllowAnonymous]
    public async Task<IActionResult> Friends([FromRoute(Name = "id")] string profileId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Friendship>(pageNumber,
            pageSize,
            x => x.Friend.FirstName.Contains(search) || x.Friend.LastName.Contains(search),
            x => x.StartedAtUtc,
            desc);

        var result = await _mediator.Send(new GetFriendshipsPageQuery(profileId, pageRequest, User.GetId()));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/remove")]
    public async Task<IActionResult> CancelFriendship([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new CancelFriendshipCommand(User.GetId()!, profileId));

        return this.GetFromResult(result);
    }
    #endregion // Friendship

    [HttpGet("profile/groups")]
    public async Task<IActionResult> Groups([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Group>(pageNumber,
            pageSize,
            x => x.Name.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetGroupsPageForQuery(User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}/groups")]
    public async Task<IActionResult> ProfileGroups([FromRoute(Name = "id")] string profileId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Group>(pageNumber,
            pageSize,
            x => x.Name.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetGroupsPageForQuery(profileId, pageRequest, User.GetId()));

        return this.GetFromResult(result);
    }

    [HttpGet("invites")]
    public async Task<IActionResult> Invites([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Invite>(pageNumber,
            pageSize,
            x => x.Group.Name.Contains(search),
            x => x.SentAtUtc,
            desc);

        var result = await _mediator.Send(new GetInvitesPageQuery(User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    #region Saved Post
    [HttpPost("save/{id}")]
    public async Task<IActionResult> SavePost(string id, [FromQuery] string? groupId)
    {
        var result = await _mediator.Send(new SavePostCommand(User.GetId()!, id, groupId));

        return this.GetFromResult(result);
    }

    [HttpDelete("save/{id}")]
    public async Task<IActionResult> UnsavePost(string id)
    {
        var result = await _mediator.Send(new UnsavePostCommand(User.GetId()!, id));

        return this.GetFromResult(result);
    }
    #endregion // Saved Post

    [AllowAnonymous]
    [HttpGet("auth")]
    public async Task<IActionResult> Auth()
    {
        Console.WriteLine("____________");
        return Challenge("oidc");
    }
}
