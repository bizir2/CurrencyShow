using Finance.Api.Middlewares;
using Finance.Api.Services;
using Finance.Application;
using Finance.Application.Abstractions;
using Finance.Application.Configuration;
using Finance.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<UserIdMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
