using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.DeleteProfile;

public sealed record DeleteProfileCommand(string Id) : ICommand<Profile>;