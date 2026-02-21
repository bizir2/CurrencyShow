using MediatR;

namespace Identity.Application.Users.Commands;

public record RegisterUserCommand(string Login, string Name, string Password) : IRequest<Guid>;