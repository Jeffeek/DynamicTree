using DynamicTree.Persistence.Migrations;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicTree.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ApplicationDbContext>(
                options => options.UseLazyLoadingProxies()
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
                    .ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS))
                    .EnableSensitiveDataLogging()
                /*, ServiceLifetime.Transient*/);

        services.AddScoped(p => p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

        services.AddMigrations(configuration);

        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;

        var migrationRunner = scopedServices.GetRequiredService<IMigrationRunner>();
        migrationRunner.MigrateUp();

        using var db = scopedServices.GetRequiredService<ApplicationDbContext>();

        db.Database.EnsureCreated();

        return services;
    }
}