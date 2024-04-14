using MediatR;
using PR2.Shared.Common;

namespace SocialMediaService.Application.Interfaces;

public interface ICommand<TResponse> : IRequest<Result<TResponse>> where TResponse : notnull;