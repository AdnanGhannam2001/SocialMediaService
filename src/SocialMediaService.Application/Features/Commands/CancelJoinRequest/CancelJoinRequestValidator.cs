using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.CancelJoinRequest;

public sealed class CancelJoinRequestValidator : AbstractValidator<CancelJoinRequestCommand>
{
    public CancelJoinRequestValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}