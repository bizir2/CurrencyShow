using Identity.Application.Abstractions;
using MediatR;

namespace Identity.Application.Users.Commands;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
    : IRequestHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByLoginAsync(request.Login, cancellationToken);

        if (user is null)
        {
            throw new Exception("Логин не найден");
        }

        bool isPasswordValid = passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new Exception("Не верный пароль");
        }

        return jwtProvider.Generate(user);
    }
}