using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeleteComment;

public sealed class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, Result<Comment>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public DeleteCommentHandler(IProfileRepository profileRepo, IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Comment>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var post = request.ParentId is null
            ? await _postRepo.GetWithCommentAsync(request.PostId, request.CommentId, cancellationToken)
            : await _postRepo.GetWithReplyAsync(request.PostId, request.ParentId, request.CommentId, cancellationToken);

        if (post is null || !await PostHelper.CanAccessPostAsync(_profileRepo, profile, post, cancellationToken))
        {
            return new RecordNotFoundException("Post is not found");
        }

        if (post.Comments.Count == 0 || (request.ParentId is not null && post.Comments.ElementAt(0).Replies.Count == 0))
        {
            return new RecordNotFoundException("Comment is not found");
        }

        var comment = request.ParentId is null
            ? post.Comments.ElementAt(0)
            : post.Comments.ElementAt(0).Replies.ElementAt(0);

        if (comment.ProfileId != post.ProfileId && comment.ProfileId != profile.Id)
        {
            return new UnauthorizedException("You should be the owner of either the post or the comment");
        }
        
        if (request.ParentId is null)
        {
            post.RemoveComment(comment);
        }
        else
        {
            post.Comments.ElementAt(0).RemoveReply(comment);
        }

        await _postRepo.SaveChangesAsync(cancellationToken);

        return comment;
    }
}
