using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.DeleteFromBlockedList;

public class DeleteFromBlockedListValidator : AbstractValidator<DeleteFromBlockedListCommand>
{
    public DeleteFromBlockedListValidator()
    {
        RuleFor(x => x.BlockerId)
            .NotEmpty()
            .NotEqual(x => x.BlockedId);

        RuleFor(x => x.BlockedId)
            .NotEmpty();
    }
}