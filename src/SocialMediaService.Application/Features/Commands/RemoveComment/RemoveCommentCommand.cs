using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Commands.RemoveComment;

public sealed record RemoveCommentCommand(string PostId, string? ParentId, string CommentId, string ProfileId) : ICommand<Comment>;