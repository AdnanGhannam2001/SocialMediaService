using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetKickedPage;

public sealed class GetKickedPageHandler : IRequestHandler<GetKickedPageQuery, Result<Page<Kicked>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public GetKickedPageHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Page<Kicked>>> Handle(GetKickedPageQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, profile.Id, cancellationToken);

        if (group is null)
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.Members.Count == 0 || group.Members.ElementAt(0).Role != MemberRoleTypes.Admin)
        {
            return new UnauthorizedException("Only Admins in this group can access this");
        }

        var page = await _groupRepo.GetKickedPageAsync(request.GroupId, request.Request, cancellationToken);

        return page;
    }
}
