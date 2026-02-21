using Identity.Application;
using Identity.Application.Users.Commands;
using Identity.Infrastructure;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Identity.Application.Abstractions;
using Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Shared.Options;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity.Tests;

public class AuthTest
{
    private ServiceProvider _serviceProvider;
    private IdentityDbContext _context;
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
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        services.AddScoped<IJwtProvider, JwtProvider>();

        var context = new DefaultHttpContext();
        _httpContextAccessor = new HttpContextAccessor { HttpContext = context };
        services.AddSingleton(_httpContextAccessor);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
        });

        configure?.Invoke(services);

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<IdentityDbContext>();
    }

    [Test]
    public async Task Register_ShouldCreateUser_And_ExistInDb()
    {
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var command = new RegisterUserCommand("testuser", "Test User", "password123");

        var userId = await mediator.Send(command, CancellationToken.None);

        var user = await _context.Users.FindAsync(userId);
        Assert.NotNull(user);
        Assert.That(user.Login, Is.EqualTo(command.Login));
    }
    
    [Test]
    public async Task Register_ShouldCreateUser_And_Error_DublicateLogin()
    {
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var command = new RegisterUserCommand("testuser", "Test User", "password123");
        
        await mediator.Send(command, CancellationToken.None);
        
        Assert.ThrowsAsync<Exception>(async () => await mediator.Send(command, CancellationToken.None));
    }

    [Test]
    public async Task Login_ShouldSetCookie_WhenCredentialsAreValid()
    {
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var registerCommand = new RegisterUserCommand("loginuser", "Login User", "password123");
        var userId = await mediator.Send(registerCommand, CancellationToken.None);

        var loginCommand = new LoginUserCommand("loginuser", "password123");
        var token = await mediator.Send(loginCommand, CancellationToken.None);

        var jwtOptions = _serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userIdClaim = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

        Assert.That(Guid.Parse(userIdClaim), Is.EqualTo(userId));
    }
}