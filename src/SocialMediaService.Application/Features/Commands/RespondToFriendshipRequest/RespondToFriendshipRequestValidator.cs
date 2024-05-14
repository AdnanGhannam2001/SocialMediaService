using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.RespondToFriendshipRequest;

public class RespondToFriendshipRequestValidator : AbstractValidator<RespondToFriendshipRequestCommand>
{
    public RespondToFriendshipRequestValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty()
            .NotEqual(x => x.ReceiverId);

        RuleFor(x => x.ReceiverId)
            .NotEmpty();
    }
}