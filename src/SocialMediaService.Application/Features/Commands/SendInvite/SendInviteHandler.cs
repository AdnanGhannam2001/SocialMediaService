using MassTransit;
using MediatR;
using PR2.Contracts.Events;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.SendInvite;

public sealed class SendInviteHandler : IRequestHandler<SendInviteCommand, Result<Invite>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;
    private readonly IPublishEndpoint _publisher;

    public SendInviteHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo,
        IPublishEndpoint publisher)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
        _publisher = publisher;
    }

    public async Task<Result<Invite>> Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetWithInviteAsync(request.ProfileId, request.GroupId, request.SenderId, cancellationToken);

        if (profile is null || await ProfileHelper.IsBlocked(_profileRepo, request.ProfileId, request.SenderId, cancellationToken))
        {
            return new RecordNotFoundException("Profile is not found or blocked");
        }

        if (profile.ReceivedInvites.Count > 0)
        {
            return new DuplicatedRecordException("You've already sent an invite to this user to join this group");
        }

        var sender = await _profileRepo.GetByIdAsync(request.SenderId, cancellationToken);

        if (sender is null)
        {
            return new RecordNotFoundException("Sender profile is not found");
        }

        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, request.SenderId, cancellationToken);

        if (group is null)
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.Members.Count == 0 || group.Members.ElementAt(0).Role != group.Settings.InviterRole)
        {
            return new UnauthorizedException("You can't send an invite to this group");
        }

        if (await _groupRepo.CountMembersAsync(group.Id, x => x.ProfileId.Equals(profile.Id), cancellationToken) > 0)
        {
            return new DuplicatedRecordException("User is already a member in this group");
        }

        var invite = new Invite(group, profile, sender, request.Content);
        group.AddInvite(invite);
        await _groupRepo.SaveChangesAsync(cancellationToken);

        var notification = new NotifyEvent(sender.Id,
            $"You got an invite to '{group.Name}' group from {sender.FirstName}",
            $"group/{group.Id}");
        await _publisher.Publish(notification, cancellationToken);

        return invite;
    }
}
