using MassTransit;
using MediatR;
using PR2.Contracts.Events;
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
    private readonly IPublishEndpoint _publisher;

    public ReactToPostHandler(IProfileRepository profileRepo,
        IPostRepository postRepo,
        IPublishEndpoint publisher)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
        _publisher = publisher;
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

        var notification = new NotifyEvent(post.Profile.Id,
            $"{profile.FirstName} reacted on your post",
            $"profiles/{profile.Id}");
        await _publisher.Publish(notification, cancellationToken);

        return reaction;
    }
}
