using MediatR;
using PR2.Shared.Common;

namespace SocialMediaService.Application.Interfaces;

public interface IQuery<T> : IRequest<Result<T>> where T : notnull;