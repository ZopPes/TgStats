using TgStats.Core.Domain;
using static TgStats.user.Domain.UserEntity;

namespace TgStats.user.Domain;

public class UserEntity : IEntity<Id>
{
    public Id ID { get; set; }
    public Password Password { get; set; }

    public readonly record struct Id(Guid Value) : IId<Id>
    {
        public override string? ToString() => Value.ToString();
    };
}