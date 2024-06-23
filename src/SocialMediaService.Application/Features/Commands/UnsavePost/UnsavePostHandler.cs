using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.UnsavePost;

public sealed class UnsavePostHandler
    : IRequestHandler<UnsavePostCommand, Result<SavedPost>>
{
    private readonly IProfileRepository _profileRepo;

    public UnsavePostHandler(IProfileRepository profileRepo)
    {
        _profileRepo = profileRepo;
    }

    public async Task<Result<SavedPost>> Handle(UnsavePostCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetWithSavedPostAsync(request.ProfileId, request.PostId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        if (profile.SavedPosts.Count == 0)
        {
            return new RecordNotFoundException("Post is not in saved");
        }

        var savedPost = profile.SavedPosts.ElementAt(0);
        profile.UnsavePost(savedPost);
        await _profileRepo.SaveChangesAsync(cancellationToken);

        return savedPost;
    }
}
