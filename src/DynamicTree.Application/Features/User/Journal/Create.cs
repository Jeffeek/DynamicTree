using DynamicTree.Domain.Enums;
using DynamicTree.Persistence;
using DynamicTree.SharedKernel.Exceptions;
using DynamicTree.SharedKernel.Features.User.Journal;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DynamicTree.Application.Features.User.Journal;

public class CreateHandler : IRequestHandler<CreateRequest, Unit>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateHandler(IDbContextFactory<ApplicationDbContext> dbContextFactory, IHttpContextAccessor httpContextAccessor)
    {
        _dbContextFactory = dbContextFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(CreateRequest request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync();

        await db.Set<Domain.Entities.Journal>().AddAsync(new Domain.Entities.Journal
        {
            EventId = GetRequestId(),
            Text = GenerateText(request)
        }, cancellationToken);

        await db.CommitTransactionAsync();

        return Unit.Value;
    }

    private string GenerateText(CreateRequest request)
        => JsonConvert.SerializeObject(new
        {
            RequestId = _httpContextAccessor.HttpContext.Connection.Id,
            _httpContextAccessor.HttpContext.Request.Path,
            RequestInfo = request.Request,
            request.Exception.StackTrace,
            Type = GetJournalType(request.Exception)
        });

    private long GetRequestId() => long.Parse(_httpContextAccessor.HttpContext.Connection.Id[..17]);

    private static JournalType GetJournalType(Exception exception)
        => exception switch
        {
            BadRequestException or ValidationException or NotFoundException => JournalType.Secure,
            _ => JournalType.Exception
        };
}