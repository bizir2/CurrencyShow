using Finance.Application.Abstractions;
using Finance.Application.Currencies.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Api.Controllers;

[Authorize]
[ApiController]
[Route("currency")]
public class CurrencyController(
    IMediator mediator,
    ICurrentUserService currentUserService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCurrencies(CancellationToken ct)
    {
        Guid userId = currentUserService.UserId!.Value;
        
        var query = new GetCurrenciesQuery(userId);
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}