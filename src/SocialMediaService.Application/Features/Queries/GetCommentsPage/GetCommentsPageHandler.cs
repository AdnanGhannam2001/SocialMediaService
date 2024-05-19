using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetCommentsPage;

public sealed class GetCommentsPageHandler : IRequestHandler<GetCommentsPageQuery, Result<Page<Comment>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public GetCommentsPageHandler(IProfileRepository profileRepo, IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }
    public async Task<Result<Page<Comment>>> Handle(GetCommentsPageQuery request, CancellationToken cancellationToken)
    {
        var profile = request.ProfileId is not null
            ? await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken)
            : null;

        if (request.ProfileId is not null && profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var post = request.ParentId is null
            ? await _postRepo.GetByIdAsync(request.PostId, cancellationToken)
            : await _postRepo.GetWithCommentAsync(request.PostId, request.ParentId, cancellationToken);

        if (post is null || !await PostHelper.CanAccessPostAsync(_profileRepo, profile, post, cancellationToken))
        {
            return new RecordNotFoundException("Post is not found");
        }

        if (request.ParentId is not null && post.Comments.Count == 0)
        {
            return new RecordNotFoundException("Comment is not found");
        }

        var comments = await _postRepo.GetCommentsPageAsync(request.PostId, request.ParentId, request.Request, cancellationToken);

        return comments;
    }
}
