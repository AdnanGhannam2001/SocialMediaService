using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.UpdateSettings;

public sealed record UpdateSettingsCommand(string Id, Settings Settings) : ICommand<Settings>;