using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.AddToBlockList;

public sealed record AddToBlockListCommand(string BlockerId, string ProfileId, string? Reason = null) : ICommand<Block>;