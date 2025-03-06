using TgStats.user.Domain;

namespace TgStats.User.Infrastructure.Auth;

public interface IClaimService
{
    UserEntity.Id GetUserId();
}
