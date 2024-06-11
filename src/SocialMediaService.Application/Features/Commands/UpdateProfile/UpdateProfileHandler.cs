using MediatR;
using Microsoft.Extensions.Logging;
using Npgsql;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Persistent.Interfaces;

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

        try
        {
            // TODO: Fix: profile.JobInformation.StartDate is getting updated
            profile.Update(request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Gender,
                request.PhoneNumber,
                request.Bio,
                request.JobInformations,
                request.Socials);

            await _repo.SaveChangesAsync(cancellationToken);

            return profile;
        }
        catch (Exception exp)
        {
            _logger.LogError("[Application] {Message}\n{StackTrace}", exp.Message, exp.StackTrace);

            if (exp is ExceptionBase eb)
            {
                return eb;
            }

            if (exp is PostgresException pe)
            {
                return new DataValidationException(pe.ColumnName ?? pe.ConstraintName ?? pe.ErrorCode.ToString(), pe.MessageText);
            }

            return new UnexpectedException();
        }
    }
}
