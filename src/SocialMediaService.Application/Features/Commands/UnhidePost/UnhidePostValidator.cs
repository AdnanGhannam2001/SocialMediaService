using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.UnhidePost;

public sealed class UnhidePostValidator : AbstractValidator<UnhidePostCommand>
{
    public UnhidePostValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}