using FluentValidation;

namespace SocialMediaService.Application.Features.Commands.DeleteFromFavoriteDiscussions;

public sealed class DeleteFromFavoriteDiscussionsValidator : AbstractValidator<DeleteFromFavoriteDiscussionsCommand>
{
    public DeleteFromFavoriteDiscussionsValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();

        RuleFor(x => x.DiscussionId)
            .NotEmpty();
    }
}