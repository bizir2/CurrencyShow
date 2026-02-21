using System.Xml.Serialization;
using Finance.Application.Abstractions;
using Finance.Application.Abstractions.Crb;
using Finance.Infrastructure.Configuration;

namespace Finance.Infrastructure.Integration;

public class CbrService(IHttpClientFactory httpClientFactory) : ICbrService
{
    public async Task<CurrencyRate[]> GetRatesAsync(CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(CbrClientConfigurationExtensions.HttpClientName);
        
        var response = await httpClient.GetAsync("scripts/XML_daily.asp", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Не удалость отправить запрос {CbrClientConfigurationExtensions.HttpClientName}");
        }

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        var xmlSerializer = new XmlSerializer(typeof(CurrencyResponse));
        
        var getCurrenciesResponse = (CurrencyResponse?) xmlSerializer.Deserialize(stream);
        if (getCurrenciesResponse == null || getCurrenciesResponse?.Currencies.Length > 0)
        {
            throw new Exception($"Не удалось распарсить ответ {CbrClientConfigurationExtensions.HttpClientName}");
        }

        return getCurrenciesResponse!.Currencies;
    }
}