using Identity.Domain;

namespace Identity.Application.Abstractions;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct);
    Task<User?> GetByLoginAsync(string login, CancellationToken ct);
}