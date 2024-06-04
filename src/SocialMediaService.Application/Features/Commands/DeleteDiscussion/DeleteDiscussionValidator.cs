using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.DeleteDiscussion;

public sealed class DeleteDiscussionValidator : AbstractValidator<DeleteDiscussionCommand>
{
    public DeleteDiscussionValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.DiscussionId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}