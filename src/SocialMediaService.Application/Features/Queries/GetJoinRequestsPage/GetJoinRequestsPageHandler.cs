using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetJoinRequestsPage;

public sealed class GetJoinRequestsPageHandler : IRequestHandler<GetJoinRequestsPageQuery, Result<Page<JoinRequest>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public GetJoinRequestsPageHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Page<JoinRequest>>> Handle(GetJoinRequestsPageQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.RequesterId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, profile.Id, cancellationToken);

        if (group is null || (group.Members.Count == 0 && group.Visibility == GroupVisibilities.Hidden))
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.Members.ElementAt(0).Role != MemberRoleTypes.Admin)
        {
            return new UnauthorizedException("Only group's admins can access this");
        }
        
        var page = await _groupRepo.GetJoinRequestsPageAsync(request.GroupId, request.Request, cancellationToken);

        return page;
    }
}
