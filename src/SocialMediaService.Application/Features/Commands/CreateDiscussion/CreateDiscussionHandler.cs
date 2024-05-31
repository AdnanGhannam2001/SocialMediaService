using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.CreateDiscussion;

public sealed class CreateDiscussionHandler : IRequestHandler<CreateDiscussionCommand, Result<Discussion>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public CreateDiscussionHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Discussion>> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, request.ProfileId, cancellationToken);

        if (group is null || (group.Visibility == GroupVisibilities.Hidden && group.Members.Count == 0))
        {
            return new RecordNotFoundException("Group is not found");
        }

        var member = group.Members.ElementAt(0);

        if (group.Settings.PostingRole < member.Role)
        {
            return new UnauthorizedException($"You should have role of {Enum.GetName(group.Settings.PostingRole)} or higher to post in this group");
        }

        var discussion = new Discussion(group, profile, request.Title, request.Content, request.Tags.Select(x => new Tag(x)));
        group.AddDiscussion(discussion);
        await _groupRepo.SaveChangesAsync(cancellationToken);

        return discussion;
    }
}
