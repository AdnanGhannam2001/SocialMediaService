using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.CancelJoinRequest;

public sealed class CancelJoinRequestHandler : IRequestHandler<CancelJoinRequestCommand, Result<JoinRequest>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public CancelJoinRequestHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<JoinRequest>> Handle(CancelJoinRequestCommand request, CancellationToken cancellationToken)
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

        if (group.JoinRequests.Count == 0)
        {
            return new DataValidationException("GroupId", "You didn't send a join request to this group");
        }

        var joinRequest = group.JoinRequests.ElementAt(0);
        group.RemoveJoinRequest(joinRequest);
        await _groupRepo.SaveChangesAsync(cancellationToken);

        return joinRequest;
    }
}
