using FluentValidation;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

namespace SocialMediaService.Application.Features.Commands.UpdateProfile;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .Length(3, 40);

        RuleFor(x => x.LastName)
            .Length(3, 40);

        RuleFor(x => x.DateOfBirth)
            .ExclusiveBetween(new DateTime(1925, 1, 1), DateTime.Now);

        RuleFor(x => x.Gender)
            .IsInEnum();
            
        RuleFor(x => x.Bio)
            .MaximumLength(1000);

        When(x => x.JobInformations is not null, () =>
        {
            RuleFor(x => x.JobInformations!.JobTitle)
                .MaximumLength(50);

            RuleFor(x => x.JobInformations!.Company)
                .MaximumLength(50);

            RuleFor(x => x.JobInformations!.StartDate)
                .ExclusiveBetween(new DateTime(1925, 1, 1), DateTime.Now);
        });

        RuleFor(x => x.Socials)
            .Must(x => Socials.AreValidLinks(x!))
            .When(x => x.Socials is not null);
    }
}