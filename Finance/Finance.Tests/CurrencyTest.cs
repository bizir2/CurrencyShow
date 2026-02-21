using Finance.Application.Currencies;
using Finance.Application.Currencies.Queries;
using Finance.Domain;
using Finance.Infrastructure;
using Finance.Infrastructure.Currencies;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Tests;

public class CurrencyTest
{
    private ServiceProvider _serviceProvider;
    private CurrencyDbContext _context;
    private IHttpContextAccessor _httpContextAccessor;

    [SetUp]
    public void Setup()
    {
        ConfigureServices(services => {});
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _serviceProvider.Dispose();
    }

    private void ConfigureServices(Action<IServiceCollection> configure)
    {
        var services = new ServiceCollection();
        services.AddDbContext<CurrencyDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();

        var context = new DefaultHttpContext();
        _httpContextAccessor = new HttpContextAccessor { HttpContext = context };
        services.AddSingleton(_httpContextAccessor);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(GetCurrenciesQuery).Assembly);
        });

        configure?.Invoke(services);

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<CurrencyDbContext>();
    }

    [Test]
    public async Task GetCurrencies_ShouldReturnCurrencies_ForUser()
    {
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var userId = Guid.NewGuid();
        
        var currency1 = new Currency
        {
            Id = Guid.NewGuid(),
            CurrencyCode = "USD",
            Name = "US Dollar",
            Rate = 75.0m
        };

        var currency2 = new Currency
        {
            Id = Guid.NewGuid(),
            CurrencyCode = "EUR",
            Name = "Euro",
            Rate = 85.0m
        };

        var userCurrency = new UserCurrency
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Currency = currency1
        };

        _context.Currency.AddRange(currency1, currency2);
        _context.UserCurrency.Add(userCurrency);
        await _context.SaveChangesAsync();

        var query = new GetCurrenciesQuery(userId);
        var result = await mediator.Send(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].CurrencyCode, Is.EqualTo("USD"));
    }
}