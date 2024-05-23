using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetGroup;

public sealed class GetGroupHandler : IRequestHandler<GetGroupQuery, Result<Group>>
{
    private readonly IGroupRepository _groupRepo;

    public GetGroupHandler(IGroupRepository groupRepo)
    {
        _groupRepo = groupRepo;
    }

    public async Task<Result<Group>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
    {
        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, request.RequesterId ?? string.Empty, cancellationToken);

        if (group is null || (group.Members.Count == 0 && group.Visibility == GroupVisibilities.Hidden))
        {
            return new RecordNotFoundException("Group is not found");
        }

        return group;
    }
}