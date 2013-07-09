namespace RepositoryMagic
{
    public interface IModel<TId>
    {
        TId Id { get; }
    }
}
