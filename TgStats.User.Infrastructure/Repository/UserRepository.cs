
using Microsoft.EntityFrameworkCore;
using TgStats.user.Domain;

namespace TgStats.User.Infrastructure.Repository;

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
    {
        return context.Set<UserEntity>().SingleOrDefaultAsync(x => x.ID.Equals(iD),token);
    }
}
