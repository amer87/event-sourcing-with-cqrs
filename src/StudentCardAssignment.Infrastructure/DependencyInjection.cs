using Microsoft.Extensions.DependencyInjection;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Infrastructure.Repositories;
using StudentCardAssignment.Infrastructure.Persistence;

namespace StudentCardAssignment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register IApplicationDbContext interface
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Add read model repositories
        services.AddScoped<IStudentReadModelRepository, StudentReadModelRepository>();
        services.AddScoped<ICardReadModelRepository, CardReadModelRepository>();

        // Add event-sourced repositories
        services.AddScoped<IEventSourcedCardRepository, EventSourcedCardRepository>();
        services.AddScoped<IEventSourcedStudentRepository, EventSourcedStudentRepository>();

        return services;
    }
}
