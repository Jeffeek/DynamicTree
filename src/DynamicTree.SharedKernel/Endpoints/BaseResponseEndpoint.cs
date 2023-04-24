using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DynamicTree.SharedKernel.Endpoints;

[Route("api")]
public abstract class BaseResponseEndpoint<TResponse, TEndpoint> : BaseAsyncEndpoint<TResponse>
{
    private IMediator? _mediator;
    private ILogger? _logger;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<TEndpoint>>()!;
}