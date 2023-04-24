using DynamicTree.SharedKernel.Features.User.Journal;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DynamicTree.SharedKernel.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private readonly IMediator _mediator;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DynamicTree Request: Unhandled Exception for Request {Name} {Request}", typeof(TRequest).Name, JsonConvert.SerializeObject(request));

            if (request is not CreateRequest)
                await _mediator.Send(new CreateRequest
                {
                    Exception = ex,
                    Request = JsonConvert.SerializeObject(request)
                }, cancellationToken);

            throw;
        }
    }
}