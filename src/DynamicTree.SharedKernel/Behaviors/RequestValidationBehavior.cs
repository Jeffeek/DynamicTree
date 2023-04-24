using FluentValidation;
using MediatR;

namespace DynamicTree.SharedKernel.Behaviors;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var context = ValidationContext<TRequest>.CreateWithOptions(request, vs => vs.IncludeAllRuleSets());

        var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .Where(v => !v.IsValid)
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Count != 0) throw new ValidationException(failures);

        return await next();
    }
}