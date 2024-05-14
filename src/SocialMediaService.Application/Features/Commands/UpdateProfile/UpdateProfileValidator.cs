using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.UpdateProfile;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 40);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 40);

        RuleFor(x => x.DateOfBirth)
            .ExclusiveBetween(new DateTime(1925, 1, 1), DateTime.Now);

        RuleFor(x => x.Gender)
            .IsInEnum();
            
        RuleFor(x => x.Bio)
            .MaximumLength(1000);

        RuleFor(x => x.JobInformations!.JobTitle)
            .MaximumLength(50);

        RuleFor(x => x.JobInformations!.Company)
            .MaximumLength(50);

        RuleFor(x => x.JobInformations!.StartDate)
            .ExclusiveBetween(new DateTime(1925, 1, 1), DateTime.Now);
    }
}