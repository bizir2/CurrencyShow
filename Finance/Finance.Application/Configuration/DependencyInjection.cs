using Finance.Application.Currencies.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Application.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg
                .RegisterServicesFromAssembly(typeof(GetCurrenciesQuery).Assembly)
        );
        return services;
    }
}