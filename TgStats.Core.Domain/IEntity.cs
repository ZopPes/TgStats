namespace TgStats.Core.Domain;

public interface IEntity<TId>
    where TId : unmanaged, IId<TId>
{
    public TId ID { get; }
}
