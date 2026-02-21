using Shared.Primitives;

namespace Finance.Domain;

public class Currency : BaseEntity
{
    public required string CurrencyCode { get; set; }
    public required string Name { get; set; }
    public required decimal Rate { get; set; }
}