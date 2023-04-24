using DynamicTree.Application.Services;
using DynamicTree.SharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicTree.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeService, UtcDateTimeService>();

        return services;
    }
}