using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetGroupsPage;

public sealed class GetGroupsPageHandler : IRequestHandler<GetGroupsPageQuery, Result<Page<Group>>>
{
    private readonly IGroupRepository _groupRepo;

    public GetGroupsPageHandler(IGroupRepository groupRepo)
    {
        _groupRepo = groupRepo;
    }

    public async Task<Result<Page<Group>>> Handle(GetGroupsPageQuery request, CancellationToken cancellationToken)
    {
        var pageRequest = new PageRequest<Group>(request.Request.PageNumber,
                request.Request.PageSize,
                x => x.Visibility != GroupVisibilities.Hidden && (x.Name.Contains(request.Search) || x.Description.Contains(request.Search)),
                request.Request.KeySelector);

        var page = await _groupRepo.GetPageAsync(pageRequest, cancellationToken);

        return page;
    }
}
