using DynamicTree.Domain.Entities;
using DynamicTree.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DynamicTree.Application.Features.User.Tree.Node;

public class DeleteRequestValidator : AbstractValidator<DeleteRequest>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public DeleteRequestValidator(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;

        RuleFor(p => p.NodeId)
            .GreaterThan(0)
            .MustAsync(NodeExists).WithMessage(r => $"Parent node with Id {r.NodeId} not exists")
            .MustAsync(ChildrenAreDeleted).WithMessage("You have to delete all children nodes first");
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

    private async Task<bool> ChildrenAreDeleted(long id, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return !await db.Set<TreeNode>().AnyAsync(x => x.ParentNodeId == id, cancellationToken);
    }
}