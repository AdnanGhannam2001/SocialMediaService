using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.FollowAccount;

public class FollowAccountValidator : AbstractValidator<FollowAccountCommand>
{
    public FollowAccountValidator()
    {
        RuleFor(x => x.FollowerId)
            .NotEmpty()
            .NotEqual(x => x.ProfileId);

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}