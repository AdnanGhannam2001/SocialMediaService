using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Commands.AddComment;

public sealed record AddCommentCommand(string PostId, string? CommentId, string ProfileId, string Content) : ICommand<Comment>;