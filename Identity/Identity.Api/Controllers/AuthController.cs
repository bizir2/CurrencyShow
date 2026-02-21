using Identity.Api.DTOs;
using Identity.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto request, CancellationToken ct)
    {
        var command = new RegisterUserCommand(request.Login, request.Name, request.Password);
        var userId = await mediator.Send(command, ct);
        return Ok(userId);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto request, CancellationToken ct)
    {
        var command = new LoginUserCommand(request.Login, request.Password);
        var token = await mediator.Send(command, ct);
        
        HttpContext.Response.Cookies.Append("jwt_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict, 
            Expires = DateTime.UtcNow.AddMinutes(15)
        });
        
        return Ok();
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("jwt_token");
        return Ok();
    }
}