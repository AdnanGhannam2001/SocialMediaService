using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Commands.DeleteComment;

public sealed record DeleteCommentCommand(string PostId, string? ParentId, string CommentId, string ProfileId) : ICommand<Comment>;