namespace TgStats.user.Domain.Repositories;

public interface IUserRepository
{
    Task Add(UserEntity newUser, CancellationToken token);
    Task<UserEntity?> Get(UserEntity.Id iD, CancellationToken token);
}