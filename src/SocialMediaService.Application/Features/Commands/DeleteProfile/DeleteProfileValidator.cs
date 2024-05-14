using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.DeleteProfile;

public class DeleteProfileValidator : AbstractValidator<DeleteProfileCommand>
{
    public DeleteProfileValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}