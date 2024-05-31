using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.AddToFavoriteDiscussions;

public sealed record AddToFavoriteDiscussionsCommand(string ProfileId, string GroupId, string DiscussionId) 
    : ICommand<FavoriteDiscussion>;