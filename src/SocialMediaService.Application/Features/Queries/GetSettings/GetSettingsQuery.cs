using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetSettings;

public sealed record GetSettingsQuery(string Id) : IQuery<Settings>;