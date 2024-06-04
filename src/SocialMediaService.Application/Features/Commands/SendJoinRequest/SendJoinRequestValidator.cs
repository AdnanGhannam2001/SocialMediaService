using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.SendJoinRequest;

public sealed class SendJoinRequestValidator : AbstractValidator<SendJoinRequestCommand>
{
    public SendJoinRequestValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .MaximumLength(300);
    }
}