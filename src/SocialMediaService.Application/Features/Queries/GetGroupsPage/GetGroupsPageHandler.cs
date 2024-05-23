using System.Linq.Expressions;
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
        Expression<Func<Group, bool>> excludeHidden = x => x.Visibility != GroupVisibilities.Hidden;

        var pageRequest = new PageRequest<Group>(request.Request.PageNumber,
                request.Request.PageSize,
                Expression.Lambda<Func<Group, bool>>(
                    Expression.AndAlso(request.Request.Predicate ?? (_ => true), excludeHidden),
                        excludeHidden.Parameters[0]),
                request.Request.KeySelector);

        var page = await _groupRepo.GetPageAsync(request.Request, cancellationToken);

        return page;
    }
}
