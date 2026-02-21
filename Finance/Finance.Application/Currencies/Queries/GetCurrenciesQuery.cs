using Finance.Application.Currencies.Dtos;
using MediatR;

namespace Finance.Application.Currencies.Queries;

public record GetCurrenciesQuery(Guid UserId) : IRequest<List<CurrencyDto>>;

public class GetCurrenciesQueryHandler(ICurrencyRepository currencyRepository)
    : IRequestHandler<GetCurrenciesQuery, List<CurrencyDto>>
{
    public async Task<List<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var currencies = await currencyRepository.GetCurrenciesByUserIdAsync(request.UserId, cancellationToken);
        return currencies.Select(c => new CurrencyDto(c.Id, c.CurrencyCode, c.Name, c.Rate)).ToList();
    }
}