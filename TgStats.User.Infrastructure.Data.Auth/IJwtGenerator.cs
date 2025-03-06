using TgStats.user.Domain;

namespace TgStats.User.Infrastructure.Auth;

public interface IJwtGenerator
{
    string GenerateAccessToken(UserEntity user);
    string GenerateRefreshToken(UserEntity user);
}
