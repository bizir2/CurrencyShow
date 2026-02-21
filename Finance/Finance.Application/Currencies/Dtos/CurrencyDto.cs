namespace Finance.Application.Currencies.Dtos;

public record CurrencyDto(Guid Id, string CurrencyCode, string Name, decimal Rate);