using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.UnsavePost;

public sealed class UnsavePostValidator : AbstractValidator<UnsavePostCommand>
{
    public UnsavePostValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.PostId)
            .NotEmpty();
    }
}