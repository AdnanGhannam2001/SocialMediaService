using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.UnfollowAccount;

public class UnfollowAccountValidator : AbstractValidator<UnfollowAccountCommand>
{
    public UnfollowAccountValidator()
    {
        RuleFor(x => x.FollowerId)
            .NotEmpty()
            .NotEqual(x => x.FollowedId);

        RuleFor(x => x.FollowedId)
            .NotEmpty();
    }
}