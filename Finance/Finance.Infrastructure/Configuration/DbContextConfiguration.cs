using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Infrastructure.Configuration;

public static class DbContextConfiguration
{
    public static IServiceCollection AddCurrencyDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CurrencyDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), opts =>
            {
                opts.MigrationsAssembly("Finance.Infrastructure");
            });
        });
        
        return services;
    }
}