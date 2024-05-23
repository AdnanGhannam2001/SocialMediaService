using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.SendJoinRequest;

public sealed class SendJoinRequestHandler : IRequestHandler<SendJoinRequestCommand, Result<JoinRequest>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public SendJoinRequestHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<JoinRequest>> Handle(SendJoinRequestCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithJoinRequestAsync(request.GroupId, request.ProfileId, cancellationToken);

        if (group is null || group.Visibility == GroupVisibilities.Hidden)
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.JoinRequests.Count > 0)
        {
            return new DataValidationException("GroupId", "You've already send a join request to this group");
        }

        if ((await _groupRepo.GetWithMemebershipAsync(request.GroupId, request.ProfileId, cancellationToken))!.Members.Count > 0)
        {
            return new DataValidationException("GroupId", "You're already a member in this group");
        }

        if ((await _groupRepo.GetWithKickedAsync(request.GroupId, request.ProfileId, cancellationToken))!.Kicked.Count > 0)
        {
            return new DataValidationException("GroupId", "You've got kicked out of this group before, so you can't join without an invitation");
        }

        var joinRequest = new JoinRequest(group, profile, request.Content);
        group.AddJoinRequest(joinRequest);
        await _groupRepo.SaveChangesAsync(cancellationToken);

        return joinRequest;
    }
}
