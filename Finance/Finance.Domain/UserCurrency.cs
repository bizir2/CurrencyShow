using Shared.Primitives;

namespace Finance.Domain;

public class UserCurrency: BaseEntity
{
    public required Guid UserId { get; set; }
    public required Currency Currency { get; set; }
}