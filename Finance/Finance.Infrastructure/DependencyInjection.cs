using Finance.Application.Currencies;
using Finance.Infrastructure.Configuration;
using Finance.Infrastructure.Currencies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddCurrencyDbContext(configuration)
            .AddCrb(configuration);

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();

        return services;
    }
}