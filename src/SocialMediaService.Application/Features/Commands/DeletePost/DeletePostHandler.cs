using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeletePost;

public sealed class DeletePostHandler : IRequestHandler<DeletePostCommand, Result<Post>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public DeletePostHandler(IProfileRepository profileRepo,
        IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
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
            return new UnauthorizedException("You can't delete other's posts");
        }

        await _postRepo.DeleteAsync(post, cancellationToken);

        return post;
    }
}
