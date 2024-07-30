using MassTransit;
using MediatR;
using PR2.Contracts.Events;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.AddComment;

public sealed class AddCommentHandler : IRequestHandler<AddCommentCommand, Result<Comment>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;
    private readonly IPublishEndpoint _publisher;

    public AddCommentHandler(IProfileRepository profileRepo,
        IPostRepository postRepo,
        IPublishEndpoint publisher)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
        _publisher = publisher;
    }

    public async Task<Result<Comment>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var post = request.CommentId is null
            ? await _postRepo.GetByIdAsync(request.PostId, cancellationToken)
            : await _postRepo.GetWithCommentAsync(request.PostId, request.CommentId, cancellationToken);

        if (post is null || !await PostHelper.CanAccessPostAsync(_profileRepo, profile, post, cancellationToken))
        {
            return new RecordNotFoundException("Post is not found");
        }

        Comment comment;

        // Comment on Post
        if (request.CommentId is null)
        {
            comment = new Comment(post, profile, request.Content);
            post.AddComment(comment);
        }
        // Reply to Comment
        else
        {
            if (post.Comments.Count == 0)
            {
                return new RecordNotFoundException("Comment is not found");
            }

            var parent = post.Comments.ElementAt(0);
            comment = new Comment(parent, profile, request.Content);
            parent.AddReply(comment);

            var notification = new NotifyEvent(post.ProfileId, $"{profile.FirstName} Commented on your post", $"");
            await _publisher.Publish(notification, cancellationToken);
        }

        await _postRepo.SaveChangesAsync(cancellationToken);

        return comment;
    }
}
