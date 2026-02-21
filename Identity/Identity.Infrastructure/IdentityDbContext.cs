using Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;
public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        modelBuilder.HasPostgresExtension("uuid-ossp");
    }
}