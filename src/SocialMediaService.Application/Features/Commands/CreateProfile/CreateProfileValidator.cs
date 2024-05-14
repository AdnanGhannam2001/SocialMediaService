using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.CreateProfile;

public class CreateProfileValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileValidator()
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
    }
}