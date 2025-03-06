using Microsoft.EntityFrameworkCore;
using TgStats.user.Domain;
using TgStats.user.Domain.Repositories;
using TgStats.User.Infrastructure.Data.EF;

namespace TgStats.User.Infrastructure.Repositories;

public class UserRepository(
    UserDbContext context
    ) : IUserRepository
{
    private readonly UserDbContext context = context;

    public async Task Add(UserEntity newUser, CancellationToken token)
    {
        await context.AddAsync(newUser, token);
        await context.SaveChangesAsync(token);
    }

    public Task<UserEntity?> Get(UserEntity.Id iD, CancellationToken token) 
        => context.Set<UserEntity>()
        .SingleOrDefaultAsync(x => x.ID.Equals(iD), token);
}
