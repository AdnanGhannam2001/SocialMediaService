using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.HidePost;

public sealed class HidePostHandler : IRequestHandler<HidePostCommand, Result<Post>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public HidePostHandler(IProfileRepository profileRepo, IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Post>> Handle(HidePostCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var post = await _postRepo.GetWithHiddenAsync(request.PostId, request.ProfileId, cancellationToken);

        if (post is null || !await PostHelper.CanAccessPostAsync(_profileRepo, profile, post, cancellationToken))
        {
            return new RecordNotFoundException("Post is not found");
        }

        if (post.HiddenBy.Count > 0)
        {
            return new DuplicatedRecordException("Post is already hidden");
        }

        post.HideTo(profile);
        await _postRepo.SaveChangesAsync(cancellationToken);

        return post;
    }
}
