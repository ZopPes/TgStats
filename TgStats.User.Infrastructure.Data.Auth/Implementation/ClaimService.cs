using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TgStats.user.Domain;
using TgStats.User.Infrastructure.Auth;

namespace TgStats.User.Infrastructure.Auth.Implementation;

/// <summary>
/// Сервис для работы с данными пользователя из jwt.
/// </summary>
public class ClaimService(ClaimsPrincipal userClaims) : IClaimService
{
    private readonly ClaimsPrincipal userClaims=userClaims;

    public UserEntity.Id GetUserId()
        => userClaims
            .FindFirst(JwtRegisteredClaimNames.Sub)
            ?.Value is { } userIdRaw
            && Guid.TryParse(userIdRaw, out var userId)
            ? new(userId)
            : throw new Exception();
}
