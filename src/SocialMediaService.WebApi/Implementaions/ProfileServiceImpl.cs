using Grpc.Core;
using MediatR;
using SocialMediaService.Application.Features.Commands.CreateProfile;
using SocialMediaService.Application.Features.Commands.DeleteProfile;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.WebApi.Protos;

namespace SocialMediaService.WebApi.Implementaions;

public sealed class ProfileServiceImpl : ProfileService.ProfileServiceBase
{
    private readonly IMediator _mediator;

    public ProfileServiceImpl(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<Empty> CreateProfile(CreateProfileRequest request, ServerCallContext context)
    {
        if (await _mediator.Send(new CreateProfileCommand(request.Id,
            request.FirstName,
            request.LastName,
            request.DateOfBirth.ToDateTime(),
            (Domain.Enums.Genders) request.Gender,
            string.IsNullOrEmpty(request.PhoneNumber) ? null : new PhoneNumber(request.PhoneNumber)))
                is var result && result == false)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Exceptions.First().ToString()));
        }

        return new();
    }

    public override async Task<Empty> DeleteProfile(DeleteProfileRequest request, ServerCallContext context)
    {
        if (await _mediator.Send(new DeleteProfileCommand(request.Id)) is var result && result == false)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Exceptions.First().ToString()));
        }

        return new();
    }
}