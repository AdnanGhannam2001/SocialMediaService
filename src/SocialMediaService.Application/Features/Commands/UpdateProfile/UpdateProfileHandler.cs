using MediatR;
using Microsoft.Extensions.Logging;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;
using SocialMediaService.Persistent.Repositories;

namespace SocialMediaService.Application.Features.Commands.UpdateProfile;

public sealed class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result<Profile>>
{
    private readonly IProfileRepository _repo;
    private readonly ILogger<UpdateProfileHandler> _logger;

    public UpdateProfileHandler(IProfileRepository repo, ILogger<UpdateProfileHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Result<Profile>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetByIdAsync(request.Id, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException($"Profile with Id: {request.Id} is not found");
        }

        profile.Update(request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.PhoneNumber,
            request.Bio,
            request.JobInformations,
            request.Socials);

        try
        {
            await _repo.SaveChangesAsync(cancellationToken);

            return profile;
        }
        catch (Exception exp)
        {
            _logger.LogCritical("[Application] {Message}\n{StackTrace}", exp.Message, exp.StackTrace);
            return new UnexpectedException();
        }
    }
}