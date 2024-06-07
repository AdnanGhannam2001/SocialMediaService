using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetPostsPage;

public sealed class GetPostsPageHandler : IRequestHandler<GetPostsPageQuery, Result<Page<Post>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public GetPostsPageHandler(IProfileRepository profileRepo,
        IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Page<Post>>> Handle(GetPostsPageQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var page = await _postRepo.GetPageWithoutHiddenAsync(request.ProfileId, request.Request, cancellationToken);

        return page;
    }
}
