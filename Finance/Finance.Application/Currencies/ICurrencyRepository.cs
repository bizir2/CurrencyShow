using Finance.Domain;

namespace Finance.Application.Currencies;

public interface ICurrencyRepository
{
    Task<List<Currency>> GetCurrenciesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task UpsertCurrenciesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}