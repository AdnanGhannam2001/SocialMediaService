using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.DeletePost;

public sealed class DeletePostValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.PostId)
            .NotEmpty();
    }
}