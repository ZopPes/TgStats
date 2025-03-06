namespace TgStats.Core.Domain;

public interface IId<TId>
    where TId : unmanaged, IId<TId>
{

}