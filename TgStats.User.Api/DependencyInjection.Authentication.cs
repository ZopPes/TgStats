using Microsoft.AspNetCore.Authentication.Cookies;
using TgStats.User.Infrastructure.Auth.Implementation;
using TgStats.User.Infrastructure.Auth;
using TokenOptions = TgStats.User.Infrastructure.Auth.Models.TokenOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

internal static partial class DependencyInjection
{
    internal static IApplicationBuilder UserAuth(this IApplicationBuilder app)
       => app.UseAuthentication()
        .UseAuthorization();
    internal static IServiceCollection InitAuth(this IServiceCollection services, IConfiguration configuration)
       => services.Configure<TokenOptions>(configuration.GetSection(nameof(TokenOptions)))
       .AddScoped<IJwtGenerator, JwtGenerator>()
       .AddScoped<IClaimService, ClaimService>()
       .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
       .AddTransient(services =>
       services
       .GetRequiredService<IHttpContextAccessor>()
       .HttpContext?.User
       ?? throw new InvalidOperationException("HttpContext or User is null"))
       .AddJwtAuthentication(configuration)
       .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/Account/Login";
           options.AccessDeniedPath = "/Account/AccessDenied";
       })
       .Services
       .AddAuthorization()
       .AddHttpClient();

    internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var typedPublicKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var rawPublicKey = configuration["TokenOptions:RefreshTokenPublicKeyEcdsaP256"]
            ?? throw new Exception();
        typedPublicKey
            .ImportSubjectPublicKeyInfo(
            Convert.FromBase64String(rawPublicKey), out _);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new ECDsaSecurityKey(typedPublicKey),
                    ValidIssuer = configuration["TokenOptions:TokenIssuerName"],
                    ValidAudience = configuration["TokenOptions:TokenAudienceName"],
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[
                            configuration["TokenOptions:RefreshTokenCookieName"]
                            ?? throw new Exception()];
                        return Task.CompletedTask;
                    }
                };
                options.MapInboundClaims = false;
            });

        return services;
    }
}
