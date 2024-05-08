using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.UpdateSettings;

public sealed class UpdateSettingsHandler : IRequestHandler<UpdateSettingsCommand, Result<Settings>>
{
    private readonly IProfileRepository _repo;

    public UpdateSettingsHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Settings>> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _repo.GetSettingsAsync(request.Id, cancellationToken);

        if (settings is null)
        {
            return new RecordNotFoundException("Profile not found");
        }

        settings.Update(request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.Phone,
            request.JobTitle,
            request.Company,
            request.StartDate,
            request.Socials,
            request.Bio);

        await _repo.SaveChangesAsync(cancellationToken);

        return settings;
    }
}
