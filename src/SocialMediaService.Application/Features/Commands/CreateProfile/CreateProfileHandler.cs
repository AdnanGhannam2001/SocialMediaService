using MediatR;
using Microsoft.Extensions.Logging;
using Npgsql;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.CreateProfile;

public sealed class CreateProfileHandler : IRequestHandler<CreateProfileCommand, Result<Profile>>
{
    private readonly IProfileRepository _repo;
    private readonly ILogger<CreateProfileHandler> _logger;

    public CreateProfileHandler(IProfileRepository repo, ILogger<CreateProfileHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Result<Profile>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var settings = new Settings(request.Id);

        var profile = new Profile(request.Id, 
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            settings,
            request.PhoneNumber);

        try
        {
            _logger.LogInformation("[Application] Trying to create profile with Id: {Id}", profile.Id);

            // EVENT: Account/Profile Created
            await _repo.AddAsync(profile, cancellationToken);

            _logger.LogInformation("[Application] Create profile with Id: {Id}", profile.Id);

            return profile;
        }
        catch (Exception exp)
        {
            _logger.LogError("[Application] {Message}\n{StackTrace}", exp.Message, exp.StackTrace);

            if (exp is PostgresException pe)
            {
                return new DataValidationException(pe.ColumnName ?? pe.ConstraintName ?? pe.ErrorCode.ToString(), pe.MessageText);
            }

            return new UnexpectedException();
        }
    }
}
