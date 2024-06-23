using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.SavePost;

public sealed class SavePostHandler
    : IRequestHandler<SavePostCommand, Result<SavedPost>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;
    private readonly IGroupRepository _groupRepo;

    public SavePostHandler(IProfileRepository profileRepo,
        IPostRepository postRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<SavedPost>> Handle(SavePostCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        if (request.GroupId is not null)
        {
            var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, request.ProfileId, cancellationToken);

            if (group is null)
            {
                return new RecordNotFoundException("Group is not found");
            }

            if (group.Members.Count == 0)
            {
                if (group.Visibility == GroupVisibilities.Hidden) return new RecordNotFoundException("Group is not found");
                if (group.Visibility == GroupVisibilities.Private) return new UnauthorizedException("This group is private");
            }
        }

        var post = await _postRepo.GetByIdAsync(request.PostId);

        if (post is null)
        {
            return new RecordNotFoundException("Post is not found");
        }

        var savedPost = new SavedPost(profile, post);
        profile.SavePost(savedPost);
        await _profileRepo.SaveChangesAsync(cancellationToken);

        return savedPost;
    }
}
