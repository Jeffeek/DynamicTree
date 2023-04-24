using AutoMapper;
using AutoMapper.QueryableExtensions;
using DynamicTree.Application.Features.User.Journal.Models;
using DynamicTree.Persistence;
using DynamicTree.SharedKernel.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DynamicTree.Application.Features.User.Journal;

public class GetHandler : IRequestHandler<GetRequest, JournalInfo>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public GetHandler(IDbContextFactory<ApplicationDbContext> dbContextFactory, IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<JournalInfo> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Set<Domain.Entities.Journal>()
            .Where(x => x.Id == request.Id)
            .ProjectTo<JournalInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Not found Journal with Id {request.Id}");
    }
}