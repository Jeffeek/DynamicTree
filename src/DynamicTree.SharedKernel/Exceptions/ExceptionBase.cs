using System.Net;

namespace DynamicTree.SharedKernel.Exceptions;

public class ExceptionBase : Exception
{
    public ExceptionBase(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public ExceptionBase(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}