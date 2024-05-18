using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.ReactToPost;

public sealed class ReactToPostHandler : IRequestHandler<ReactToPostCommand, Result<Reaction>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public ReactToPostHandler(IProfileRepository profileRepo, IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Reaction>> Handle(ReactToPostCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var post = await _postRepo.GetWithReactionAsync(request.PostId, request.ProfileId, cancellationToken);

        if (post is null || !await PostHelper.CanAccessPostAsync(_profileRepo, profile, post, cancellationToken))
        {
            return new RecordNotFoundException("Post is not found");
        }

        Reaction reaction;

        if (post.Reactions.Count > 0)
        {
            reaction = post.Reactions.ElementAt(0);
            post.Unreact(reaction);
            reaction.Update(request.Type);
        }
        else
        {
            reaction = new Reaction(post, profile, request.Type);
        }

        post.React(reaction);
        await _postRepo.SaveChangesAsync(cancellationToken);

        return reaction;
    }
}
