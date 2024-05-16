using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaService.Application.Features.Commands.CreatePost;
using SocialMediaService.Application.Features.Commands.DeletePost;
using SocialMediaService.Application.Features.Commands.UpdatePost;
using SocialMediaService.WebApi.Dtos.PostDtos;
using SocialMediaService.WebApi.Extensions;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        // TODO: Handle Media
        var result = await _mediator.Send(new CreatePostCommand(User.GetId()!, request.Content, request.Visibility));

        return this.GetFromResult(result, 201);
    }


    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] CreateRequest request)
    {
        // TODO: Handle Media
        var result = await _mediator.Send(new UpdatePostCommand(User.GetId()!, id, request.Content, request.Visibility));

        return this.GetFromResult(result);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await _mediator.Send(new DeletePostCommand(User.GetId()!, id));

        return this.GetFromResult(result);
    }
}