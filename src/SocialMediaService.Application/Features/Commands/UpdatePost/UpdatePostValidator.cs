using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.UpdatePost;

public sealed class UpdatePostValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .MaximumLength(1000);
    }
}