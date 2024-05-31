using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.DeleteDiscussion;

public sealed record DeleteDiscussionCommand(string GroupId, string DiscussionId, string ProfileId) : ICommand<Discussion>;