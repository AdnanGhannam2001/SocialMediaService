using SocialMediaService.Application.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetProfile;

public sealed record GetProfileQuery(string ProfileId, bool CheckOwnership = true, string? RequesterId = null)
    : IQuery<GetProfileResult>;