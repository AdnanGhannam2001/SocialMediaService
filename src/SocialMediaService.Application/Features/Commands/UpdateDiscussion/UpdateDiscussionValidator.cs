using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.UpdateDiscussion;

public sealed class UpdateDiscussionValidator : AbstractValidator<UpdateDiscussionCommand>
{
    public UpdateDiscussionValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.DiscussionId)
            .NotEmpty();

        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(1000);

        RuleForEach(x => x.Tags)
            .NotEmpty()
            .MaximumLength(30);
    }
}