using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SkipSmart.Application.Abstractions.Behaviors;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application;

public static class DependencyInjection {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        
        services.AddTransient<PasswordHasherService>();
        
        return services;
    }
}