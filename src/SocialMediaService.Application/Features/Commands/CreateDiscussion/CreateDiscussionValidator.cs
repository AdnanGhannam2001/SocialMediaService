using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.CreateDiscussion;

public sealed class CreateDiscussionValidator : AbstractValidator<CreateDiscussionCommand>
{
    public CreateDiscussionValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.GroupId)
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