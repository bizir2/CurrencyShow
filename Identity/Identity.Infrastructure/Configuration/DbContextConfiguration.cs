using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Configuration;

public static class DbContextConfiguration
{
    public static IServiceCollection AddCurrencyDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), opts =>
            {
                opts.MigrationsAssembly("Identity.Infrastructure");
            });
        });
        
        return services;
    }
}