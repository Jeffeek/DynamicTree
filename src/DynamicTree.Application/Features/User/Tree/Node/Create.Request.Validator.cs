using DynamicTree.Domain.Entities;
using DynamicTree.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DynamicTree.Application.Features.User.Tree.Node;

public class CreateRequestValidator : AbstractValidator<CreateRequest>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public CreateRequestValidator(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;

        RuleFor(p => p.ParentNodeId)
            .NotEqual(0)
            .MustAsync(ParentExists).WithMessage(r => $"Parent node with Id {r.ParentNodeId} not exists");
        RuleFor(p => p.NodeName)
            .NotEmpty()
            .MaximumLength(512);
        RuleFor(p => p.TreeName)
            .NotEmpty()
            .MaximumLength(512)
            .MustAsync(TreeNameIsCorrect).WithMessage(r => $"Root node with name {r.TreeName} not exists")
            .MustAsync((r, _, ct) => NodeNameIsUnique(r, ct)).WithMessage("Duplicate name");
    }

    private async Task<bool> TreeNameIsCorrect(string treeName, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Set<TreeNode>().AnyAsync(x => x.Name == treeName && x.ParentNodeId == null, cancellationToken);
    }

    private async Task<bool> NodeNameIsUnique(CreateRequest request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return !await db.Set<TreeNode>().AnyAsync(x => x.Name == request.NodeName && x.ParentNodeId == request.ParentNodeId, cancellationToken);
    }

    private async Task<bool> ParentExists(long id, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Set<TreeNode>().AnyAsync(x => x.Id == id, cancellationToken);
    }
}