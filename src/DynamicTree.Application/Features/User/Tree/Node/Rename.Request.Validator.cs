using DynamicTree.Domain.Entities;
using DynamicTree.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DynamicTree.Application.Features.User.Tree.Node;

public class RenameRequestValidator : AbstractValidator<RenameRequest>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public RenameRequestValidator(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;

        RuleFor(p => p.NodeId)
            .GreaterThan(0)
            .MustAsync(NodeExists).WithMessage(r => $"Node with Id {r.NodeId} not exists");
        RuleFor(p => p.NewNodeName)
            .NotEmpty()
            .MaximumLength(512)
            .MustAsync((r, _, ct) => NewNodeNameIsUnique(r, ct)).WithMessage("New name must be unique");
        RuleFor(p => p.TreeName)
            .NotEmpty()
            .MaximumLength(512)
            .MustAsync(TreeNameIsCorrect).WithMessage(r => $"Root node with name {r.TreeName} not exists");
    }

    private async Task<bool> TreeNameIsCorrect(string treeName, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Set<TreeNode>().AnyAsync(x => x.Name == treeName && x.ParentNodeId == null, cancellationToken);
    }

    private async Task<bool> NodeExists(long id, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Set<TreeNode>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    private async Task<bool> NewNodeNameIsUnique(RenameRequest request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Set<TreeNode>().AnyAsync(x => x.Id == request.NodeId && x.ParentNode!.Children.All(y => y.Name != request.NewNodeName), cancellationToken);
    }
}