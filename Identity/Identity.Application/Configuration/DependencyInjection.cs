using Identity.Application.Users.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg
                .RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly)
                .RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly)
        );
        return services;
    }
}