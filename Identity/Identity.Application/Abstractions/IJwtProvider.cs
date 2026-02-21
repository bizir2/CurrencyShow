using Identity.Domain;

namespace Identity.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}