using MediatR;
using Microsoft.Extensions.Logging;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Exceptions;
using SocialMediaService.Application.Features.Commands.UpdateProfile;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeleteProfile;

public sealed class DeleteProfileHandler : IRequestHandler<DeleteProfileCommand, Result<Profile>>
{
    private readonly IProfileRepository _repo;
    private readonly ILogger<UpdateProfileHandler> _logger;

    public DeleteProfileHandler(IProfileRepository repo, ILogger<UpdateProfileHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Result<Profile>> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetByIdAsync(request.Id, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException($"Profile with Id: {request.Id} is not found");
        }

        try
        {
            await _repo.DeleteAsync(profile, cancellationToken);

            return profile;
        }
        catch (Exception exp)
        {
            _logger.LogCritical("[Application] {Message}\n{StackTrace}", exp.Message, exp.StackTrace);
            return new UnexpectedException();
        }
    }
}
