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
using SocialMediaService.Application.Features.Commands.SendFriendshipRequest;
using SocialMediaService.Application.Features.Commands.UnfollowAccount;
using SocialMediaService.Application.Features.Commands.UpdateProfile;
using SocialMediaService.Application.Features.Commands.UpdateSettings;
using SocialMediaService.Application.Features.Queries.GetBlockedPage;
using SocialMediaService.Application.Features.Queries.GetFollowsPage;
using SocialMediaService.Application.Features.Queries.GetFriendshipRequestsPage;
using SocialMediaService.Application.Features.Queries.GetFriendshipsPage;
using SocialMediaService.Application.Features.Queries.GetProfile;
using SocialMediaService.Application.Features.Queries.GetSettings;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.WebApi.Dtos.ProfileDtos;
using SocialMediaService.WebApi.Extensions;

namespace SocialMediaService.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Profile
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _mediator.Send(new GetProfileQuery(User.GetId()!));

        return this.GetFromResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetProfileQuery(id, true, User.GetId()));

        return this.GetFromResult(result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateRequest request)
    {
        var result = await _mediator.Send(new UpdateProfileCommand(User.GetId()!,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.PhoneNumber,
            request.Bio,
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
    public async Task<IActionResult> UpdateSettings(UpdateSettingsRequest request)
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
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<Block>(pageNumber,
            pageSize,
            x => x.Blocked.FirstName.Contains(search) || x.Blocked.LastName.Contains(search),
            x => x.BlockedAtUtc);

        var result = await _mediator.Send(new GetBlockedPageQuery(User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/block")]
    public async Task<IActionResult> Block([FromRoute(Name = "id")] string profileId, [FromBody] string reason)
    {
        var result = await _mediator.Send(new AddToBlockListCommand(User.GetId()!, profileId, reason));

        return this.GetFromResult(result);
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
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<Follow>(pageNumber,
            pageSize,
            x => x.Followed.FirstName.Contains(search) || x.Followed.LastName.Contains(search),
            x => x.FollowedAtUtc);

        var result = await _mediator.Send(new GetFollowsPageQuery(User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpGet("{id}/following")]
    [AllowAnonymous]
    public async Task<IActionResult> Following([FromRoute] string id,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<Follow>(pageNumber,
            pageSize,
            x => x.Follower.FirstName.Contains(search) || x.Follower.LastName.Contains(search),
            x => x.FollowedAtUtc);

        var result = await _mediator.Send(new GetFollowsPageQuery(id, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/follow")]
    public async Task<IActionResult> Follow([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new FollowAccountCommand(User.GetId()!, profileId));

        return this.GetFromResult(result);
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
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<FriendshipRequest>(pageNumber,
            pageSize,
            x => x.Receiver.FirstName.Contains(search) || x.Receiver.LastName.Contains(search),
            x => x.SentAtUtc);

        var result = await _mediator.Send(new GetFriendshipRequestsPageQuery(User.GetId()!, true, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpGet("received")]
    public async Task<IActionResult> ReceivedFriendshipRequests([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<FriendshipRequest>(pageNumber,
            pageSize,
            x => x.Sender.FirstName.Contains(search) || x.Sender.LastName.Contains(search),
            x => x.SentAtUtc);

        var result = await _mediator.Send(new GetFriendshipRequestsPageQuery(User.GetId()!, false, pageRequest));

        return this.GetFromResult(result);
    }
    
    [HttpPost("{id}/request")]
    public async Task<IActionResult> SendFriendshipRequest([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new SendFriendshipRequestCommand(User.GetId()!, profileId));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/request")]
    public async Task<IActionResult> CancelFriendshipRequest([FromRoute(Name = "id")] string profileId)
    {
        var result = await _mediator.Send(new CancelFriendshipRequestCommand(User.GetId()!, profileId));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/respond")]
    public async Task<IActionResult> Respond([FromRoute(Name = "id")] string profileId, bool aggreed = true)
    {
        var result = await _mediator.Send(new RespondToFriendshipRequestCommand(User.GetId()!, profileId, aggreed));

        return this.GetFromResult(result);
    }
    #endregion // Friendship Requestf

    #region Friendship
    [HttpGet("{id}/friends")]
    [AllowAnonymous]
    public async Task<IActionResult> Friends([FromRoute(Name = "id")] string profileId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "")
    {
        var pageRequest = new PageRequest<Friendship>(pageNumber,
            pageSize,
            x => x.Friend.FirstName.Contains(search) || x.Friend.LastName.Contains(search),
            x => x.StartedAtUtc);

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
}