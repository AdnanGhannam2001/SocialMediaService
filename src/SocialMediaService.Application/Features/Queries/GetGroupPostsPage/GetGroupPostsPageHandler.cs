using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetGroupPostsPage;

public sealed class GetGroupPostsPageHandler : IRequestHandler<GetGroupPostsPageQuery, Result<Page<Post>>>
{
    private readonly IGroupRepository _groupRepo;

    public GetGroupPostsPageHandler(IGroupRepository groupRepo)
    {
        _groupRepo = groupRepo;
    }

    public async Task<Result<Page<Post>>> Handle(GetGroupPostsPageQuery request, CancellationToken cancellationToken)
    {
        var group = request.RequesterId is not null
            ? await _groupRepo.GetWithMemebershipAsync(request.GroupId, request.RequesterId, cancellationToken)
            : await _groupRepo.GetByIdAsync(request.GroupId, cancellationToken);

        if (group is null || (group.Visibility == GroupVisibilities.Hidden && group.Members.Count == 0))
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.Visibility == GroupVisibilities.Private && group.Members.Count == 0)
        {
            return new DataValidationException("Membership", "You have to be a member to view group's posts");
        }

        var page = await _groupRepo.GetPostsPageAsync(request.GroupId, request.Request, request.RequesterId, cancellationToken);

        return page;
    }
}
