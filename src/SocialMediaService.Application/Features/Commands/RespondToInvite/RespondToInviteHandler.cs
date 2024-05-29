using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.RespondToInvite;

public sealed class RespondToInviteHandler : IRequestHandler<RespondToInviteCommand, Result<Invite>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public RespondToInviteHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Invite>> Handle(RespondToInviteCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetWithInviteAsync(request.ProfileId, request.GroupId, request.SenderId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        if (profile.ReceivedInvites.Count == 0)
        {
            return new RecordNotFoundException("You didn't receive an invite to this group");
        }

        var group = await _groupRepo.GetByIdAsync(request.GroupId, cancellationToken);

        if (group is null)
        {
            return new RecordNotFoundException("Group is not found");
        }

        using var transaction = await _groupRepo.BeginTransactionAsync();

        try
        {
            var invite = profile.ReceivedInvites.ElementAt(0);
            profile.RemoveInvite(invite);

            if (request.Accept)
            {
                var member = new Member(group, profile);
                group.AddMember(member);
            }

            await _groupRepo.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);

            return invite;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return new TransactionFailureException("Failed to remove invite or add member");
        }
    }
}
