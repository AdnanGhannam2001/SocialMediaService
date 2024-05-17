using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetHiddenPosts;

public sealed class GetHiddenPostsHandler : IRequestHandler<GetHiddenPostsQuery, Result<Page<Post>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public GetHiddenPostsHandler(IProfileRepository profileRepo,
        IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Page<Post>>> Handle(GetHiddenPostsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var hidden = await _postRepo.GetHiddenPageAsync(profile.Id, request.Request, cancellationToken);

        return hidden;
    }
}
