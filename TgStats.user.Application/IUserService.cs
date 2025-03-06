using TgStats.user.Application.Models;

namespace TgStats.user.Application;

public interface IUserService
{
    public Task<UserCreateResult> Create(NewUserModel request, CancellationToken token);
    public Task<GetCurrentUserResult> GetCurrentUser(CancellationToken token);
    Task<LoginUserResult> Login(Credentials credentials, CancellationToken token);
}