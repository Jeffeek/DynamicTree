using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DynamicTree.Persistence.Migrations.Migrations._2023._04._24;
using NLog.Extensions.Logging;

namespace DynamicTree.Persistence.Migrations;

public static class DependencyInjection
{
    public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(CreateTreeNodeTable).Assembly).For.Migrations().For.EmbeddedResources())
            .AddLogging(lb => lb.AddFluentMigratorConsole().AddNLog());

        return services;
    }
}