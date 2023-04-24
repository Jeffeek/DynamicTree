using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DynamicTree.SharedKernel.Behaviors;

public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger;

    public RequestLogger(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Request: {Name} {Request}", typeof(TRequest).Name, JsonConvert.SerializeObject(request));

        return Task.CompletedTask;
    }
}