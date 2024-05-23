using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.SendJoinRequest;

public sealed record SendJoinRequestCommand(string GroupId, string ProfileId, string Content = "") : ICommand<JoinRequest>;