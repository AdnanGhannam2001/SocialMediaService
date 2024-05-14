using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.UpdateSettings;

public sealed class UpdateSettingsValidator : AbstractValidator<UpdateSettingsCommand>
{
    public UpdateSettingsValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.LastName).IsInEnum();
        RuleFor(x => x.DateOfBirth).IsInEnum();
        RuleFor(x => x.Gender).IsInEnum();
        RuleFor(x => x.Phone).IsInEnum();
        RuleFor(x => x.JobTitle).IsInEnum();
        RuleFor(x => x.Company).IsInEnum();
        RuleFor(x => x.StartDate).IsInEnum();
        RuleFor(x => x.Socials).IsInEnum();
        RuleFor(x => x.Bio).IsInEnum();
    }
}