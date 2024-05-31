using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.DeleteFromFavoriteDiscussions;

public sealed record DeleteFromFavoriteDiscussionsCommand(string ProfileId, string DiscussionId)
    : ICommand<FavoriteDiscussion>;