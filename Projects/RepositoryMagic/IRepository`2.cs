using System.Collections.Generic;

namespace RepositoryMagic
{
    // todo: document

    public interface IRepository<TModel, TId> where TModel : IModel<TId>
    {
        void Delete(TId id);

        bool Exists(TId id);

        TModel Find(TId id);

        TModel Get(TId id);

        IEnumerable<TModel> Get();
        
        void Insert(TModel model);

        void Update(TModel model);
    }
}
