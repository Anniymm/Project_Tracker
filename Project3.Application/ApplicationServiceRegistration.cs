using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;

namespace Project3.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
        // AutoMapper
        services.AddAutoMapper(typeof(ApplicationServiceRegistration));

        return services;
    }
}