using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using FluentValidation;
using Project3.Application.Common.Behaviors;

namespace Project3.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
        // AutoMapper - amas aghar viyeneb
        services.AddAutoMapper(typeof(ApplicationServiceRegistration));

        // validation -validators 
        // amit validatorebs vxdit xelmisawvdoms DI-it
        // Makes validators available
        services.AddValidatorsFromAssembly(assembly);
        
        // yvela requests rom validacia gauketos
        // Makes validators run automaticall
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        
        return services;
    }
}