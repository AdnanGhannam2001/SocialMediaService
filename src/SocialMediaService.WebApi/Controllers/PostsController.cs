using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR2.Shared.Common;
using SocialMediaService.Application.Features.Commands.AddComment;
using SocialMediaService.Application.Features.Commands.CreatePost;
using SocialMediaService.Application.Features.Commands.DeletePost;
using SocialMediaService.Application.Features.Commands.HidePost;
using SocialMediaService.Application.Features.Commands.ReactToPost;
using SocialMediaService.Application.Features.Commands.DeleteComment;
using SocialMediaService.Application.Features.Commands.UnhidePost;
using SocialMediaService.Application.Features.Commands.UnreactToPost;
using SocialMediaService.Application.Features.Commands.UpdatePost;
using SocialMediaService.Application.Features.Queries.GetCommentsPage;
using SocialMediaService.Application.Features.Queries.GetReactionsPage;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;
using SocialMediaService.WebApi.Dtos.PostDtos;
using SocialMediaService.WebApi.Extensions;
using SocialMediaService.Application.Features.Queries.GetPostsPage;
using SocialMediaService.Application.Features.Queries.GetProfilePosts;
using SocialMediaService.Application.Enums;
using SocialMediaService.WebApi.Services;
using MassTransit.Configuration;
using SocialMediaService.WebApi.Configurations;
using Microsoft.Extensions.Options;

namespace SocialMediaService.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> FollowedPosts([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
       var pageRequest = new PageRequest<Post>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetPostsPageQuery(User.GetId()!, pageRequest));

        return this.GetFromResult(result);
    }

    [HttpGet("friends")]
    public async Task<IActionResult> FriendsPosts([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
       var pageRequest = new PageRequest<Post>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetPostsPageQuery(User.GetId()!, pageRequest, PostsTypes.FriendsPosts));

        return this.GetFromResult(result);
    }

    [HttpGet("saved-posts")]
    public async Task<IActionResult> SavedPosts([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
       var pageRequest = new PageRequest<Post>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetPostsPageQuery(User.GetId()!, pageRequest, PostsTypes.SavedPosts));

        return this.GetFromResult(result);
    }


    [HttpGet("profile")]
    public async Task<IActionResult> MyPosts([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
       var pageRequest = new PageRequest<Post>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetProfilePostsQuery(User.GetId()!, pageRequest, User.GetId()!));

        return this.GetFromResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ProfilePosts([FromRoute(Name = "id")] string profileId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
       var pageRequest = new PageRequest<Post>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetProfilePostsQuery(profileId, pageRequest, User.GetId()!));

        return this.GetFromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreatePostRequest request,
        IFormFile? file,
        [FromServices] FilesService filesService,
        [FromServices] IOptions<Storage> storage)
    {
        var options = storage.Value.FilesOptions["PostsMedia"];

        if (file is not null)
        {
            var fileValidationResult = filesService.Validate(file, options);

            if (!fileValidationResult.IsSuccess)
            {
                return this.GetFromResult(fileValidationResult);
            }
        }

        var mediaType = filesService.GetFileMediaType(file);
        var result = await _mediator.Send(new CreatePostCommand(User.GetId()!,
            request.Content,
            request.Visibility,
            mediaType));

        if (file is not null && result.IsSuccess)
        {
            await filesService.SaveFileAsync(file, options, result.Value.Id);
        }

        return this.GetFromResult(result, 201);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id,
        [FromForm] CreatePostRequest request,
        IFormFile? file,
        [FromServices] FilesService filesService,
        [FromServices] IOptions<Storage> storage)
    {
        var options = storage.Value.FilesOptions["PostsMedia"];

        if (file is not null)
        {
            var fileValidationResult = filesService.Validate(file, options);

            if (!fileValidationResult.IsSuccess)
            {
                return this.GetFromResult(fileValidationResult);
            }
        }

        var mediaType = filesService.GetFileMediaType(file);
        var result = await _mediator.Send(new UpdatePostCommand(User.GetId()!,
            id,
            request.Content,
            request.Visibility,
            mediaType));

        if (file is not null && result.IsSuccess)
        {
            filesService.DeleteFile(options.Path, result.Value.Id);
            await filesService.SaveFileAsync(file, options, result.Value.Id);
        }

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id,
        [FromServices] IOptions<Storage> options,
        [FromServices] FilesService filesService)
    {
        var result = await _mediator.Send(new DeletePostCommand(User.GetId()!, id));

        if (result.IsSuccess && result.Value.Media is not null)
        {
            filesService.DeleteFile(options.Value.FilesOptions["PostsMedia"].Path, result.Value.Id);
        }

        return this.GetFromResult(result);
    }

    [HttpGet("hidden")]
    public async Task<IActionResult> Hidden([FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
       var pageRequest = new PageRequest<Post>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetPostsPageQuery(User.GetId()!, pageRequest, PostsTypes.HiddenPosts));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/hide")]
    public async Task<IActionResult> Hide([FromRoute(Name = "id")] string postId)
    {
        var result = await _mediator.Send(new HidePostCommand(User.GetId()!, postId));
        
        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/hide")]
    public async Task<IActionResult> Unhide([FromRoute(Name = "id")] string postId)
    {
        var result = await _mediator.Send(new UnhidePostCommand(User.GetId()!, postId));
        
        return this.GetFromResult(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}/reactions")]
    public async Task<IActionResult> Reactions([FromRoute(Name = "id")] string postId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] ReactionTypes type = ReactionTypes.Like,
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Reaction>(pageNumber,
            pageSize,
            x => x.Type.Equals(type),
            x => x.ReactedAtUtc,
            desc);

        var result = await _mediator.Send(new GetReactionsPageQuery(postId, pageRequest, User.GetId()));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}/react")]
    public async Task<IActionResult> React([FromRoute(Name = "id")] string postId,
        [FromQuery] ReactionTypes type = ReactionTypes.Like)
    {
        var result = await _mediator.Send(new ReactToPostCommand(User.GetId()!, postId, type));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/react")]
    public async Task<IActionResult> Unreact([FromRoute(Name = "id")] string postId)
    {
        var result = await _mediator.Send(new UnreactToPostCommand(User.GetId()!, postId));

        return this.GetFromResult(result);
    }


    [AllowAnonymous]
    [HttpGet("{id}/comments")]
    public async Task<IActionResult> Comments([FromRoute(Name = "id")] string postId,
        [FromQuery] string? parentId = null,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        [FromQuery] string search = "",
        [FromQuery] bool desc = true)
    {
        var pageRequest = new PageRequest<Comment>(pageNumber,
            pageSize,
            x => x.Content.Contains(search),
            x => x.CreatedAtUtc,
            desc);

        var result = await _mediator.Send(new GetCommentsPageQuery(postId, parentId, User.GetId(), pageRequest));

        return this.GetFromResult(result);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Comment([FromRoute(Name = "id")] string postId,
        [FromBody] CommentRequest dto,
        [FromQuery] string? parentId = null)
    {
        var result = await _mediator.Send(new AddCommentCommand(postId, parentId, User.GetId()!, dto.Content));

        return this.GetFromResult(result);
    }

    [HttpDelete("{id}/comments/{commentId}")]
    public async Task<IActionResult> RemoveComment([FromRoute(Name = "id")] string postId,
        [FromRoute] string commentId,
        [FromQuery] string? parentId = null)
    {
        var result = await _mediator.Send(new DeleteCommentCommand(postId, parentId, commentId, User.GetId()!));

        return this.GetFromResult(result);
    }
}