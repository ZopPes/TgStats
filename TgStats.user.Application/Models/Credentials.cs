using TgStats.user.Domain;

namespace TgStats.user.Application.Models;

public record Credentials(UserEntity.Id ID, Password Password);