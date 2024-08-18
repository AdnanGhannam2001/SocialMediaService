using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Posts.ValueObjects;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.UpdatePost;

public sealed class UpdatePostHandler : IRequestHandler<UpdatePostCommand, Result<Post>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public UpdatePostHandler(IProfileRepository profileRepo,
        IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Post>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);

        if (post is null)
        {
            return new RecordNotFoundException("Post is not found");
        }

        if (post.ProfileId != profile.Id)
        {
            return new UnauthorizedException("You can't update other's posts");
        }

        post.Update(request.Content,
            request.Visibility,
            request.MediaType is null ? null : new Media((MediaTypes) request.MediaType));

        await _postRepo.UpdateAsync(post, cancellationToken);

        return post;
    }
}
