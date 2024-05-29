using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetInvites;

public sealed class GetInvitesPageHandler : IRequestHandler<GetInvitesPageQuery, Result<Page<Invite>>>
{
    private readonly IProfileRepository _repo;

    public GetInvitesPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<Invite>>> Handle(GetInvitesPageQuery request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var page = await _repo.GetInvitesPageAsync(profile.Id, request.Request, cancellationToken);

        return page;
    }
}
