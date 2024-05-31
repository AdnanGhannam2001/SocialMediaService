using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeleteFromFavoriteDiscussions;

public sealed class DeleteFromFavoriteDiscussionsHandler
    : IRequestHandler<DeleteFromFavoriteDiscussionsCommand, Result<FavoriteDiscussion>>
{
    private readonly IProfileRepository _profileRepo;

    public DeleteFromFavoriteDiscussionsHandler(IProfileRepository profileRepo)
    {
        _profileRepo = profileRepo;
    }

    public async Task<Result<FavoriteDiscussion>> Handle(DeleteFromFavoriteDiscussionsCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetWithFavoriteDiscussionAsync(request.ProfileId, request.DiscussionId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        if (profile.FavoriteDiscussions.Count == 0)
        {
            return new RecordNotFoundException("Discussion is not in favorites list");
        }

        var favoriteDiscussion = profile.FavoriteDiscussions.ElementAt(0);
        profile.RemoveFromFavoriteDiscussions(favoriteDiscussion);
        await _profileRepo.SaveChangesAsync(cancellationToken);

        return favoriteDiscussion;
    }
}
