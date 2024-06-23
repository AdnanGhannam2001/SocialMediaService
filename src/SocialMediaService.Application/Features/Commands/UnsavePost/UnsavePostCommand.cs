using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.UnsavePost;

public sealed record UnsavePostCommand(string ProfileId, string PostId)
    : ICommand<SavedPost>;