using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.AddComment;

public sealed class AddCommentValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .MaximumLength(1000);
    }
}