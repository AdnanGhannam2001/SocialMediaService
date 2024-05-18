using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.ReactToPost;

public sealed record ReactToPostCommand(string ProfileId, string PostId, ReactionTypes Type) : ICommand<Reaction>;