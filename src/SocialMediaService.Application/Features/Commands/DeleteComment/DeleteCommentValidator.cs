using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.DeleteComment;

public sealed class DeleteCommentValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.CommentId)
            .NotEmpty();
    }
}