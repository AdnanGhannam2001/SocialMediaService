using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.CancelFriendship;

public sealed class CancelFriendshipValidator : AbstractValidator<CancelFriendshipCommand>
{
    public CancelFriendshipValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty()
            .NotEqual(x => x.FriendId);

        RuleFor(x => x.FriendId)
            .NotEmpty();
    }
}