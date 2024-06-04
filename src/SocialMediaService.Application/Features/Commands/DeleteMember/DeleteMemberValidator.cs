using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.DeleteMember;

public sealed class DeleteMemberValidator : AbstractValidator<DeleteMemberCommand>
{
    public DeleteMemberValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty();

        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.Reason)
            .MaximumLength(300);
    }
}