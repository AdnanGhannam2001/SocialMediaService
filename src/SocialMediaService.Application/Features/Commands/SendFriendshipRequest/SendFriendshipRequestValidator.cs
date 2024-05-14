using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.SendFriendshipRequest;

public class SendFriendshipRequestValidator : AbstractValidator<SendFriendshipRequestCommand>
{
    public SendFriendshipRequestValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty()
            .NotEqual(x => x.ReceiverId);

        RuleFor(x => x.ReceiverId)
            .NotEmpty();
    }
}