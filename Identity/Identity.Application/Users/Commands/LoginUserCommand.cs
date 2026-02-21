using MediatR;

namespace Identity.Application.Users.Commands;

public record LoginUserCommand(string Login, string Password) : IRequest<string>;