using System.Reflection;
using DynamicTree.Domain.Interfaces;
using DynamicTree.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DynamicTree.Persistence;

public class ApplicationDbContext : DbContext
{
    private IDbContextTransaction? _currentTransaction;
    private readonly ILogger<ApplicationDbContext> _logger;
    private readonly IDateTimeService _dateTimeService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions
        , ILogger<ApplicationDbContext> logger
        , IDateTimeService dateTimeService) : base(dbContextOptions)
    {
        _logger = logger;
        _dateTimeService = dateTimeService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        _logger.LogInformation("Start model creating");

        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        _logger.LogInformation("Finish model creating");
    }

    private void TrackChanges()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = _dateTimeService.Now;
                    break;
            }

        foreach (var entry in ChangeTracker.Entries<ICreatedAtEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = _dateTimeService.Now;
                    break;
            }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        TrackChanges();

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    public override int SaveChanges()
    {
        TrackChanges();

        var result = base.SaveChanges();

        return result;
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return;

        _currentTransaction = await Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync().ConfigureAwait(false);

            if (_currentTransaction != null)
                await _currentTransaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task CommitTransactionAsync(bool isConcurrency, CancellationToken cancellationToken)
    {
        try
        {
            if (isConcurrency)
            {
                var saved = false;
                while (!saved)
                    try
                    {
                        // Attempt to save changes to the database
                        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        foreach (var entry in ex.Entries)
                        {
                            // https://docs.microsoft.com/ru-ru/ef/core/saving/concurrency
                            var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                            if (databaseValues == null)
                            {
                                entry.State = EntityState.Added;
                                continue;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                    }
            }
            else await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (_currentTransaction != null)
                await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void BeginTransaction()
    {
        if (_currentTransaction != null) return;

        _currentTransaction = Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        try
        {
            SaveChanges();

            _currentTransaction?.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public override void Dispose()
    {
        if (_currentTransaction != null) RollbackTransaction();
        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        if (_currentTransaction != null) RollbackTransaction();
        return base.DisposeAsync();
    }
}