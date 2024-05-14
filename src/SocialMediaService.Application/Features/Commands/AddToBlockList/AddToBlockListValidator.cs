using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.AddToBlockList;

public sealed class AddToBlockListValidator : AbstractValidator<AddToBlockListCommand>
{
    public AddToBlockListValidator()
    {
        RuleFor(x => x.BlockerId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty()
            .NotEqual(x => x.BlockerId);

        RuleFor(x => x.Reason)
            .MaximumLength(300);
    }
}