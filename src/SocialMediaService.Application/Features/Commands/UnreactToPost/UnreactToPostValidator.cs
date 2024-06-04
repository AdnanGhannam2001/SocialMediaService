using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.UnreactToPost;

public sealed class UnreactToPostValidator : AbstractValidator<UnreactToPostCommand>
{
    public UnreactToPostValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}