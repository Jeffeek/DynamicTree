using AutoMapper;
using AutoMapper.EntityFrameworkCore;
using DynamicTree.Domain.Entities;
using DynamicTree.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DynamicTree.Application.Features.User.Tree.Node;

public class DeleteHandler : IRequestHandler<DeleteRequest, Unit>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public DeleteHandler(IDbContextFactory<ApplicationDbContext> dbContextFactory, IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteRequest request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await db.BeginTransactionAsync();

        await db.Set<TreeNode>()
            .Persist(_mapper)
            .RemoveAsync(request, cancellationToken);

        await db.CommitTransactionAsync();

        return Unit.Value;
    }
}