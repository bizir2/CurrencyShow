using Finance.Infrastructure.Options;
using Finance.Application.Abstractions;
using Finance.Infrastructure.Integration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Infrastructure.Configuration;

public static class CbrClientConfigurationExtensions
{
    public static string HttpClientName = "CbrHttpClient";
    public static IServiceCollection AddCrb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ICbrService, CbrService>();
        
        var options = configuration
                          .GetSection(CrbOptions.Position)
                          .Get<CrbOptions>() ??
                      throw new Exception("CrbOptions конфиг не найден");
        
        services.AddHttpClient(
            HttpClientName,
            (
                _,
                httpClient) =>
            {
                httpClient.BaseAddress = new Uri(options.BaseAddress, UriKind.RelativeOrAbsolute);
            });
        
        return services;
    }
}