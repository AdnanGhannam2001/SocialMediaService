using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.UpdateGroup;

public sealed class UpdateGroupHandler : IRequestHandler<UpdateGroupCommand, Result<Group>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public UpdateGroupHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Group>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, profile.Id, cancellationToken);

        if (group is null)
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.Members.Count == 0 || group.Members.ElementAt(0).Role != MemberRoleTypes.Admin)
        {
            return new UnauthorizedException("Only Admins in this group can perform this action");
        }

        group.Update(request.Name,
            request.Description,
            request.InviterRole,
            request.PostingRole,
            request.EditDetailsRole,
            request.Visibility,
            request.Image,
            request.CoverImage);
        await _groupRepo.SaveChangesAsync(cancellationToken);

        return group;
    }
}
