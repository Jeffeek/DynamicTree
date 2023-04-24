using MediatR;

namespace DynamicTree.SharedKernel.Features.User.Journal;

public class CreateRequest : IRequest<Unit>
{
    public Exception Exception { get; set; } = default!;
    public string Request { get; set; } = default!;
}