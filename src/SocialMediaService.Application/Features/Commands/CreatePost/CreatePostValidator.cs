using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.CreatePost;

public sealed class CreatePostValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .MaximumLength(1000);
    }
}