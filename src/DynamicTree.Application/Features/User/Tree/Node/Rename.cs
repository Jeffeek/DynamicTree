using AutoMapper;
using AutoMapper.EntityFrameworkCore;
using DynamicTree.Domain.Entities;
using DynamicTree.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DynamicTree.Application.Features.User.Tree.Node;

public class RenameHandler : IRequestHandler<RenameRequest, Unit>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public RenameHandler(IDbContextFactory<ApplicationDbContext> dbContextFactory, IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RenameRequest request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync();

        await db.Set<TreeNode>()
            .Persist(_mapper)
            .InsertOrUpdateAsync(request, cancellationToken);

        await db.CommitTransactionAsync();

        return Unit.Value;
    }
}