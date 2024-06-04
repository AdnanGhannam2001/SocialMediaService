using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.SendInvite;

public sealed class SendInviteValidator : AbstractValidator<SendInviteCommand>
{
    public SendInviteValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.SenderId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .MaximumLength(300);
    }
}