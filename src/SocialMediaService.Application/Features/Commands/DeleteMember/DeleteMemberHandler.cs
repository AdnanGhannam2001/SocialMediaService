using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeleteMember;

public sealed class DeleteMemberHandler : IRequestHandler<DeleteMemberCommand, Result<Member>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public DeleteMemberHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Member>> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
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

        var requester = await _profileRepo.GetByIdAsync(request.RequesterId, cancellationToken);

        if (requester is null)
        {
            return new RecordNotFoundException("Requester profile is not found");
        }

        var member = group.Members.ElementAt(0);

        // Check if member is organizer
        if (await _groupRepo.CountMembersAsync(group.Id,
            x => x.Role == MemberRoleTypes.Organizer && x.ProfileId.Equals(request.RequesterId),
            cancellationToken) == 0)
        {
            return new UnauthorizedException("You have to be an organizer or higher to perform this action");
        }
        
        using var transaction = await _groupRepo.BeginTransactionAsync();

        try
        {
            group.RemoveMember(member);

            if (request.Kick)
            {
                var kicked = new Kicked(group, profile, requester, request.Reason);
                group.Kick(kicked);
            }

            await _groupRepo.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return member;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return new TransactionFailureException("Failed to kick member");
        }
    }
}
