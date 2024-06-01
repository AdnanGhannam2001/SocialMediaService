using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.DeleteGroup;

public sealed record DeleteGroupCommand(string GroupId, string ProfileId) : ICommand<Group>;