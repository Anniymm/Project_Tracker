using Microsoft.Extensions.DependencyInjection;
using MediatR;
using AutoMapper;

namespace Project3.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR 
        services.AddMediatR(typeof(ApplicationServiceRegistration).Assembly);
        
        // AutoMapper
        services.AddAutoMapper(typeof(ApplicationServiceRegistration).Assembly);

        return services;
    }
}