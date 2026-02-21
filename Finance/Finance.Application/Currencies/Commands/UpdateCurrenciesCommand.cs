using System.Globalization;
using Finance.Application.Abstractions;
using Finance.Domain;
using MediatR;

namespace Finance.Application.Currencies.Commands;

public class UpdateCurrenciesCommand : IRequest;

public class UpdateCurrenciesCommandHandler(
    ICbrService cbrService,
    ICurrencyRepository currencyRepository)
    : IRequestHandler<UpdateCurrenciesCommand>
{
    public async Task Handle(UpdateCurrenciesCommand request, CancellationToken ct)
    {
        var rates = await cbrService.GetRatesAsync(ct);
        var cultureInfo = new CultureInfo("ru-RU");

        var currencies = rates.Select(r => new Currency
        {
            Id = Guid.NewGuid(),
            CurrencyCode = r.CharCode,
            Name = r.Name,
            Rate = decimal.Parse(r.ExchangeRateRaw, cultureInfo)
        }).ToList();

        await currencyRepository.UpsertCurrenciesAsync(currencies, ct);
    }
}