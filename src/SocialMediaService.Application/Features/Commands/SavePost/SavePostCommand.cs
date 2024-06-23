using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.SavePost;

public sealed record SavePostCommand(string ProfileId, string PostId, string? GroupId = null) 
    : ICommand<SavedPost>;