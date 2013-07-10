namespace RepositoryMagic.SQL
{
    public interface ISqlCommandTextBuilder<TModel, TId> where TModel : IModel<TId>
    {
        string GetItems();

        string InsertItem();

        string DeleteItem();

        string FindItem();

        string ItemExists();

        string UpdateItem();
    }
}
