using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetReactionsPage;

public sealed class GetReactionsPageHandler : IRequestHandler<GetReactionsPageQuery, Result<Page<Reaction>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public GetReactionsPageHandler(IProfileRepository profileRepo, IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Page<Reaction>>> Handle(GetReactionsPageQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);

        if (post is null)
        {
            return new RecordNotFoundException("Post is not found");
        }

        var profile = request.ProfileId is not null
            ? await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken)
            : null;

        if (!await PostHelper.CanAccessPostAsync(_profileRepo, profile, post, cancellationToken))
        {
            return new RecordNotFoundException("Post is not found");
        }

        var page = await _postRepo.GetReactionsPageAsync(request.PostId, request.Request, cancellationToken);

        return page;
    }
}
