using Domain.Interfaces;
using Infrastructure.DatabaseInitializer;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseFactory, DatabaseFactory.DatabaseFactory>();
        services.AddSingleton<IDatabaseInitializer, DatabaseInitializer.DatabaseInitializer>();
        services.AddTransient<IDataProcessor, DataProcessor>();
        services.AddTransient<ITripRepository, TripRepository>();

        return services;
    }
}