using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.HidePost;

public sealed class HidePostValidator : AbstractValidator<HidePostCommand>
{
    public HidePostValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}