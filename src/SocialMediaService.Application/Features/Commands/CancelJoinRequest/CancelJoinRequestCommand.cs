using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.CancelJoinRequest;

public sealed record CancelJoinRequestCommand(string GroupId, string ProfileId) : ICommand<JoinRequest>;