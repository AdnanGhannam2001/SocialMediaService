using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.ChangeMemberRole;

public sealed class ChangeMemberRoleValidator : AbstractValidator<ChangeMemberRoleCommand>
{
    public ChangeMemberRoleValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty();

        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}