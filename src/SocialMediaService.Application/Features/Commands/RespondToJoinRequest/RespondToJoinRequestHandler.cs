using MassTransit;
using MediatR;
using PR2.Contracts.Events;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.RespondToJoinRequest;

public sealed class RespondToJoinRequestHandler : IRequestHandler<RespondToJoinRequestCommand, Result<Unit>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;
    private readonly IPublishEndpoint _publisher;

    public RespondToJoinRequestHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo,
        IPublishEndpoint publisher)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
        _publisher = publisher;
    }

    public async Task<Result<Unit>> Handle(RespondToJoinRequestCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        // Check if Admin
        {
            var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, profile.Id, cancellationToken);

            if (group is null || (group.Members.Count == 0 && group.Visibility == GroupVisibilities.Hidden))
            {
                return new RecordNotFoundException("Group is not found");
            }

            if (group.Members.ElementAt(0).Role != MemberRoleTypes.Admin)
            {
                return new UnauthorizedException("Only group's admins can access this");
            }

        }

        var requester = await _profileRepo.GetByIdAsync(request.JoinRequesterId, cancellationToken);

        if (requester is null)
        {
            return new RecordNotFoundException("Requester profile is not found");
        }

        // Check if 'requester' sent a join request
        {
            var group = (await _groupRepo.GetWithJoinRequestAsync(request.GroupId, requester.Id, cancellationToken))!;

            if (group.JoinRequests.Count == 0)
            {
                return new RecordNotFoundException("Join request not found");
            }

            var joinRequest = group.JoinRequests.ElementAt(0);
            group.RemoveJoinRequest(joinRequest);

            if (request.Accept)
            {
                var member = new Member(group, requester);
                group.AddMember(member);

                var message = new MemberJoinedEvent(group.Id, member.ProfileId, member.Role.ToString());
                await _publisher.Publish(message, cancellationToken);
            }

            await _groupRepo.SaveChangesAsync(cancellationToken);

            var notification = new NotifyEvent(requester.Id,
                $"Your join request on {group.Name} got " + (request.Accept ? "accepted" : "rejected"),
                $"groups/{group.Id}");
            await _publisher.Publish(notification, cancellationToken);
        }

        return Unit.Value;
    }
}
