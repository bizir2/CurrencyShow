using Shared.Primitives;

namespace Identity.Domain;

public class User : BaseEntity
{
    public required string Login { get; set; }
    public required string Name { get; set; }
    public required string PasswordHash { get; set; }
}