using Finance.Application.Abstractions.Crb;

namespace Finance.Application.Abstractions;

public interface ICbrService 
{
    Task<CurrencyRate[]> GetRatesAsync(CancellationToken cancellationToken);
}