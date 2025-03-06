namespace TgStats.User.Infrastructure.Auth.Models;

public class TokenOptions
{
    public required string AccessTokenPrivateKeyEcdsaP256 { get; init; }
    public required TimeSpan AccessTokenLifetime { get; init; }
    public required string RefreshTokenPrivateKeyEcdsaP256 { get; init; }
    public required string RefreshTokenPublicKeyEcdsaP256 { get; init; }
    public required TimeSpan RefreshTokenLifetime { get; init; }
    public required string RefreshTokenCookieName { get; init; }
    public required string TokenIssuerName { get; init; }
    public required string TokenAudienceName { get; init; }
    public required TimeSpan AuthenticationCodeLifeTime { get; init; }
}
