using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.CreateDiscussion;

public sealed record CreateDiscussionCommand(string ProfileId, string GroupId, string Title, string Content, IEnumerable<string> Tags)
    : ICommand<Discussion>;