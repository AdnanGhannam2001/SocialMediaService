using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.LeaveGroup;

public sealed class LeaveGroupValidator : AbstractValidator<LeaveGroupCommand>
{
    public LeaveGroupValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}