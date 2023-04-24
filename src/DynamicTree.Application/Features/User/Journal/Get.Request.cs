using DynamicTree.Application.Features.User.Journal.Models;
using MediatR;

namespace DynamicTree.Application.Features.User.Journal;

public class GetRequest : IRequest<JournalInfo>
{
    public long Id { get; set; }
}