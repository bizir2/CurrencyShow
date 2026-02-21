using Finance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure;
public class CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : DbContext(options)
{
    public DbSet<Currency> Currency { get; set; }
    public DbSet<UserCurrency> UserCurrency { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CurrencyDbContext).Assembly);
        modelBuilder.HasPostgresExtension("uuid-ossp");
    }
}