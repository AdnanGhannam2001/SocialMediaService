using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.LeaveGroup;

public sealed class LeaveGroupHandler : IRequestHandler<LeaveGroupCommand, Result<Unit>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public LeaveGroupHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Unit>> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
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

        var member = group.Members.ElementAt(0);

        // Check if member is last admin
        if (member.Role == MemberRoleTypes.Admin
            && await _groupRepo.CountMembersAsync(group.Id, x => x.Role == MemberRoleTypes.Admin, cancellationToken) == 1)
        {
            return new DataValidationException("ProfileId", "You can't leave this group, please premote someone to admin first");
        }
        
        group.RemoveMember(member);
        await _groupRepo.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
