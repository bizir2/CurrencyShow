using Finance.Application.Currencies;
using Finance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Currencies;

public class CurrencyRepository(CurrencyDbContext currencyDbContext) : ICurrencyRepository
{
    public Task<List<Currency>> GetCurrenciesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return currencyDbContext
            .UserCurrency
            .Include(x => x.Currency)
            .Where(x => x.UserId == userId)
            .Select(x => x.Currency)
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task UpsertCurrenciesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var currencyDict = (await currencyDbContext.Currency.ToListAsync(cancellationToken)).ToDictionary(x => x.CurrencyCode);
        
        foreach (var currency in currencies)
        {
            if (currencyDict.TryGetValue(currency.CurrencyCode, out var existingCurrency))
            {
                existingCurrency.Rate = currency.Rate;
                currencyDbContext.Currency.Update(existingCurrency);
            }
            else
            {
                currencyDbContext.Currency.Update(currency);
            }
        }

        await currencyDbContext.SaveChangesAsync(cancellationToken);
    }
}