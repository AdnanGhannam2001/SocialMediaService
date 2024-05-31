using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetDiscussionsPage;

public sealed class GetDiscussionsPageHandler : IRequestHandler<GetDiscussionsPageQuery, Result<Page<Discussion>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public GetDiscussionsPageHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Page<Discussion>>> Handle(GetDiscussionsPageQuery request, CancellationToken cancellationToken)
    {
        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, request.ProfileId ?? string.Empty, cancellationToken);

        if (group is null || (group.Visibility == GroupVisibilities.Hidden && group.Members.Count == 0))
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.Visibility == GroupVisibilities.Private && group.Members.Count == 0)
        {
            return new UnauthorizedException("This group is private");
        }

        var page = await _groupRepo.GetDiscussionsPageAsync(request.GroupId, request.Request, cancellationToken);

        return page;
    }
}
