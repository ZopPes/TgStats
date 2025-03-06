using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using TgStats.user.Domain;
using TgStats.User.Infrastructure.Auth.Models;

namespace TgStats.User.Infrastructure.Auth.Implementation;

public class JwtGenerator(IOptions<TokenOptions> options) : IJwtGenerator
{
    private readonly TokenOptions options=options.Value;

    public string GenerateAccessToken(UserEntity user)
    {
        var privateKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        privateKey.ImportECPrivateKey(Convert.FromBase64String(options.AccessTokenPrivateKeyEcdsaP256), out _);

        var expireDate = DateTimeOffset.UtcNow + options.AccessTokenLifetime;
        var exp = expireDate
            .ToUnixTimeSeconds()
            .ToString();

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.ID.Value.ToString()),
            new(JwtRegisteredClaimNames.Exp, exp)
        ];
        var token = GenerateToken(privateKey, claims, expireDate);

        return token;
    }

    public string GenerateRefreshToken(UserEntity user)
    {
        var privateKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        privateKey.ImportECPrivateKey(Convert.FromBase64String(options.RefreshTokenPrivateKeyEcdsaP256), out _);

        var expireDate = DateTimeOffset.UtcNow + options.RefreshTokenLifetime;
        var exp = expireDate
            .ToUnixTimeSeconds()
            .ToString();

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.ID.Value.ToString()),
            new(JwtRegisteredClaimNames.Exp, exp, ClaimValueTypes.DateTime)
        ];
        var token = GenerateToken(privateKey, claims, expireDate);

        return token;
    }

    private string GenerateToken(ECDsa privateKey, Claim[] claims, DateTimeOffset expiresDate)
    {
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(
                new ECDsaSecurityKey(privateKey),
                SecurityAlgorithms.EcdsaSha256
            ),
            Issuer = options.TokenIssuerName,
            Audience = options.TokenAudienceName,
            Expires = expiresDate.UtcDateTime
        };

        var tokenHandler = new JsonWebTokenHandler() { SetDefaultTimesOnTokenCreation = false };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }
}
