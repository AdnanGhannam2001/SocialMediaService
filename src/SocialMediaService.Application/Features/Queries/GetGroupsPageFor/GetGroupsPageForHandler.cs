using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetGroupsPageFor;

public sealed class GetGroupsPageForHandler : IRequestHandler<GetGroupsPageForQuery, Result<Page<Group>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public GetGroupsPageForHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Page<Group>>> Handle(GetGroupsPageForQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        if (request.RequesterId is not null && request.RequesterId != profile.Id)
        {
            var requester = await _profileRepo.GetByIdAsync(request.RequesterId, cancellationToken);

            if (requester is null)
            {
                return new RecordNotFoundException("Requester profile is not found");
            }

            if (await ProfileHelper.IsBlocked(_profileRepo, profile.Id, requester.Id, cancellationToken))
            {
                return new RecordNotFoundException("Profile is not found");
            }
        }

        Expression<Func<Group, bool>> excludeHidden = x => x.Visibility != GroupVisibilities.Hidden;

        var pageRequest = request.RequesterId != profile.Id
            ? new PageRequest<Group>(request.Request.PageNumber,
                request.Request.PageSize,
                Expression.Lambda<Func<Group, bool>>(
                    Expression.AndAlso(request.Request.Predicate ?? (_ => true), excludeHidden),
                        excludeHidden.Parameters[0]),
                request.Request.KeySelector)
            : request.Request;

        var page = await _groupRepo.GetPageAsync(request.Request, cancellationToken);

        return page;
    }
}
