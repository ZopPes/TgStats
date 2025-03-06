using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using TgStats.user.Application;
using TgStats.User.Api.Endpoint.Requests;
using TgStats.User.Api.Endpoint.Responses;
using TgStats.User.Infrastructure.Auth.Models;

namespace TgStats.User.Api.Endpoint;

internal static class EndpintExtension
{
    internal static IEndpointRouteBuilder AddUserEndpoint(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("create",async (
            CreateUserRequest user,
            IUserService userService,
            CancellationToken token) =>
        {
            var result =await userService.Create(new(new(user.Password),new(user.PasswordReset)), token);
            return result.Match<Results<Ok<CreateUserResponse>,BadRequest<string>>>(
                user => TypedResults.Ok(new CreateUserResponse(user.Value)),
                error => TypedResults.BadRequest(error.Value));
        }).AllowAnonymous()
        .WithDescription("Создание нового пользователя в системе.")
        .WithSummary("Новый пользователь");
        

        endpointRouteBuilder.MapPost("login", async (
            CredentialsRequest credentials,
            IUserService service,
            IOptions<TokenOptions> options,
            HttpContext httpContext,
            CancellationToken token) =>
        {
            var tokens = await service.Login(new(new(credentials.ID),new(credentials.Password)),token);

            return tokens.Match<Results<Ok<string>, BadRequest<string>>>
            (
                result =>
                {
                    httpContext.Response
                .Cookies
                .Append(
                    options.Value.RefreshTokenCookieName,
                    result.RefreshToken,
                    new()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow + options.Value.RefreshTokenLifetime,
                    });

                    return TypedResults.Ok(result.AccessToken);
                },
                error => TypedResults.BadRequest(error.Value));
        }).AllowAnonymous()
        .WithDescription("авторизация.")
        .WithSummary("Авторизация");

        endpointRouteBuilder.MapGet("about", async (
           IUserService userService,
           CancellationToken token) =>
        {
            var result =await userService.GetCurrentUser(token);
            return result.Match<Results<Ok<UserResponse>, BadRequest<string>>>(
                user => TypedResults.Ok(new UserResponse(user.ID.Value)),
                error => TypedResults.BadRequest(error.Value));
        }).RequireAuthorization()
        .WithDescription("Информация о пользователе.")
        .WithSummary("Об о мне");

        return endpointRouteBuilder;
    }
}