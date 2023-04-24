using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DynamicTree.SharedKernel.Endpoints;

[Route("api")]
public abstract class BaseEndpoint<TRequest, TResponse, TEndpoint> : BaseAsyncEndpoint<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private IMediator? _mediator;
    private ILogger? _logger;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<TEndpoint>>()!;

    public override async Task<ActionResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        => await Mediator.Send(request, cancellationToken);
}