using Finance.Infrastructure;
using Finance.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((_, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false);
            config.AddJsonFile("appsettings.Development.json", optional: true);
            config.AddEnvironmentVariables();
        })
        .ConfigureServices((context, services) =>
        {
            services.AddCurrencyDbContext(context.Configuration);
        })
        .Build();

using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<CurrencyDbContext>();
    
    await context.Database.MigrateAsync();
    
    Console.WriteLine("Миграции успешно применены.");
}
catch (Exception ex)
{
    Console.WriteLine($"ОШИБКА МИГРАЦИИ: {ex.Message}");
    Environment.Exit(1); 
}
