using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.ReactToPost;

public sealed class ReactToPostValidator : AbstractValidator<ReactToPostCommand>
{
    public ReactToPostValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}