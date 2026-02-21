using Identity.Application.Abstractions;
using Identity.Domain;
using MediatR;

namespace Identity.Application.Users.Commands;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByLoginAsync(request.Login, cancellationToken);
        if (existingUser is not null)
        {
            throw new Exception("Пользователь с таким логином уже существует");
        }

        var passwordHash = passwordHasher.Hash(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = request.Login,
            Name = request.Name,
            PasswordHash = passwordHash
        };

        await userRepository.AddAsync(user, cancellationToken);

        return user.Id;
    }
}