using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.UpdateDiscussion;

public sealed record UpdateDiscussionCommand(string GroupId,
    string DiscussionId,
    string ProfileId,
    string? Title = null,
    string? Content = null,
    IEnumerable<string>? Tags = null)
        : ICommand<Discussion>;