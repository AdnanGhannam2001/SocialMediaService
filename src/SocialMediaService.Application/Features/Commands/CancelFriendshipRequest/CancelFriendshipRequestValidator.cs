using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.CancelFriendshipRequest;

public sealed class CancelFriendshipRequestValidator : AbstractValidator<CancelFriendshipRequestCommand>
{
    public CancelFriendshipRequestValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty()
            .NotEqual(x => x.ReceiverId);

        RuleFor(x => x.ReceiverId)
            .NotEmpty();
    }
}