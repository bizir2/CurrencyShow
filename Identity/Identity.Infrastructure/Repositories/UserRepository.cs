using Identity.Application.Abstractions;
using Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class UserRepository(IdentityDbContext dbContext) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken ct)
    {
        await dbContext.Users.AddAsync(user, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<User?> GetByLoginAsync(string login, CancellationToken ct)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Login == login, ct);
    }
}