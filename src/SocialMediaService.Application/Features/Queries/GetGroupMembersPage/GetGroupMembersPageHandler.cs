using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetGroupMembersPage;

public sealed class GetGroupMembersPageHandler : IRequestHandler<GetGroupMembersPageQuery, Result<Page<Member>>>
{
    private readonly IGroupRepository _groupRepo;

    public GetGroupMembersPageHandler(IGroupRepository groupRepo)
    {
        _groupRepo = groupRepo;
    }

    public async Task<Result<Page<Member>>> Handle(GetGroupMembersPageQuery request, CancellationToken cancellationToken)
    {
        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, request.RequesterId ?? string.Empty, cancellationToken);

        if (group is null || (group.Members.Count == 0 && group.Visibility == GroupVisibilities.Hidden))
        {
            return new RecordNotFoundException("Group is not found");
        }

        var page = await _groupRepo.GetMembersPageAsync(request.GroupId, request.Request, cancellationToken);

        return page;
    }
}
