using Microsoft.AspNetCore.Identity;
using OneOf.Types;
using TgStats.user.Application.Models;
using TgStats.user.Domain;
using TgStats.user.Domain.Repositories;
using TgStats.User.Infrastructure.Auth;

namespace TgStats.user.Application.Implementation;

public class UserService(
    IUserRepository repository,
    IPasswordHasher<UserEntity> passwordHasher,
    IJwtGenerator tokenGenerator,
    IClaimService claimService
    ) : IUserService
{
    private readonly IUserRepository repository = repository;
    private readonly IPasswordHasher<UserEntity> passwordHasher = passwordHasher;
    private readonly IClaimService claimService = claimService;

    private static bool VerifyPassword(Password code1, Password code2) 
        => code1.Equals(code2);

    public async Task<UserCreateResult> Create(NewUserModel request, CancellationToken token)
    {
        if(!VerifyPassword(request.Password, request.PasswordReset))
            return new Error<string>("Passwords do not match");

        var newUser = new UserEntity()
        {
            ID=new(Guid.CreateVersion7()),
        };

        newUser.Password = new(
            passwordHasher
            .HashPassword(newUser, request.Password.Value));

        await repository.Add(newUser, token);
        return newUser.ID;
    }

    public async Task<GetCurrentUserResult> GetCurrentUser(CancellationToken token)
    {
        var id =claimService.GetUserId();
        return  await repository.Get(id, token) is not { } user  
            ? (GetCurrentUserResult)new Error<string>("User not found")   
            : new UserModel(user.ID);
    }

    public async Task<LoginUserResult> Login(Credentials credentials, CancellationToken token)
    {
        if(await repository.Get(credentials.ID, token) is not { } user)
            return new Error<string>("User not found");

        if(passwordHasher.VerifyHashedPassword(user, user.Password.Value, credentials.Password.Value)
            is not PasswordVerificationResult.Success)
        {
            return new Error<string>("Invalid password");
        }

        return new AuthTokens(
            tokenGenerator.GenerateRefreshToken(user),
            tokenGenerator.GenerateAccessToken(user));
    }
}