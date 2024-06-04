using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.DeleteGroup;

public sealed class DeleteGroupValidator : AbstractValidator<DeleteGroupCommand>
{
    public DeleteGroupValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}