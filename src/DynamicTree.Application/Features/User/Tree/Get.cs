using AutoMapper;
using AutoMapper.EntityFrameworkCore;
using DynamicTree.Application.Features.User.Tree.Models;
using DynamicTree.Domain.Entities;
using DynamicTree.Persistence;
using DynamicTree.SharedKernel.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DynamicTree.Application.Features.User.Tree;

public class GetHandler : IRequestHandler<GetRequest, TreeNodeInfo>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public GetHandler(IDbContextFactory<ApplicationDbContext> dbContextFactory, IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<TreeNodeInfo> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var result = new TreeNodeInfo();

        var root = await db.Set<TreeNode>().Where(x => x.Name == request.Name).FirstOrDefaultAsync(cancellationToken);

        if (root == null)
        {
            await db.BeginTransactionAsync();

            root = await db.Set<TreeNode>().Persist(_mapper).InsertOrUpdateAsync(request, cancellationToken);

            await db.CommitTransactionAsync();
        }

        _mapper.Map(root, result);

        return result;
    }
}