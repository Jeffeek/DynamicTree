using Microsoft.AspNetCore.Mvc;

namespace DynamicTree.SharedKernel.Endpoints;

[ApiController]
public abstract class BaseAsyncEndpoint<TRequest, TResponse> : BaseAsyncEndpoint
{
    public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

[ApiController]
public abstract class BaseAsyncEndpoint<TResponse> : BaseAsyncEndpoint
{
    public abstract Task<ActionResult<TResponse>> HandleAsync(CancellationToken cancellationToken = default);
}

[ApiController]
public abstract class BaseAsyncEndpoint : ControllerBase
{
}