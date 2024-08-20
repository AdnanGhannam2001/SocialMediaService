using MassTransit;
using MediatR;
using PR2.Contracts.Events;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Groups.ValueObjects;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.CreateGroup;

public sealed class CreateGroupHandler : IRequestHandler<CreateGroupCommand, Result<Group>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;
    private readonly IPublishEndpoint _publisher;

    public CreateGroupHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo,
        IPublishEndpoint publisher)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
        _publisher = publisher;
    }

    public async Task<Result<Group>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var settings = new GroupSettings();
        var group = new Group(request.Name,
            request.Description,
            settings,
            request.Visibility);

        var member = new Member(group, profile, MemberRoleTypes.Admin);
        group.AddMember(member);

        await _groupRepo.AddAsync(group, cancellationToken);

        var message = new GroupCreatedEvent(group.Id, profile.Id);
        await _publisher.Publish(message, cancellationToken);

        return group;
    }
}
