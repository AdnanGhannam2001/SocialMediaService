using System.Diagnostics;
using FluentValidation;
using MediatR;
using PR2.Shared.Common;

namespace SocialMediaService.Application.Behaviors;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var validationResult = _validators
            .Select(x => x.ValidateAsync(request, cancellationToken).GetAwaiter().GetResult())
            .Aggregate((total, next) => {
                total.Errors.AddRange(next.Errors);
                return total;
            });

        if (validationResult.IsValid) {
            return await next();
        }

        var returnType = typeof(TResponse).GetGenericArguments()[0];

        var exceptions = validationResult.Errors
            .Select(x => new ExceptionBase(x.PropertyName, x.ErrorMessage, x.ErrorCode))
            .ToArray();

        var resultConstructed = typeof(Result<>).MakeGenericType(returnType);

        var result = Activator.CreateInstance(resultConstructed, new { exceptions });

        Debug.Assert(result != null);

        return (TResponse) result;
    }
}