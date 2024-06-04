using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.RespondToInvite;

public sealed class RespondToInviteValidator : AbstractValidator<RespondToInviteCommand>
{
    public RespondToInviteValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.SenderId)
            .NotEmpty();
    }
}