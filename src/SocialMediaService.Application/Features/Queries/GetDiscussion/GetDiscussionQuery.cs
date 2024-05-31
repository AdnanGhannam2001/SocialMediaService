using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetDiscussion;

public sealed record GetDiscussionQuery(string GroupId, string DiscussionId, string ProfileId) : IQuery<Discussion>;