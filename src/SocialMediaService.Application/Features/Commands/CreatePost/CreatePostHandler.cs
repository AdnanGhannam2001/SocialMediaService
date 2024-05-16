using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.CreatePost;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, Result<Post>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public CreatePostHandler(IProfileRepository profileRepo,
        IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var post = new Post(profile, request.Content, request.Visibility, request.Media);

        await _postRepo.AddAsync(post, cancellationToken);

        return post;
    }
}
