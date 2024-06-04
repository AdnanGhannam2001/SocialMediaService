using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.RespondToJoinRequest;

public sealed class RespondToJoinRequestValidator : AbstractValidator<RespondToJoinRequestCommand>
{
    public RespondToJoinRequestValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.JoinRequesterId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}