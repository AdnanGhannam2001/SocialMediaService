using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.DeleteFromBlockedList;

public sealed record DeleteFromBlockedListCommand(string BlockerId, string BlockedId) : ICommand<Block>;