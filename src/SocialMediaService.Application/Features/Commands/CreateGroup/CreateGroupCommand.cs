using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.CreateGroup;

public sealed record CreateGroupCommand(string ProfileId,
    string Name,
    string Description,
    GroupVisibilities Visibility = GroupVisibilities.Public) : ICommand<Group>;