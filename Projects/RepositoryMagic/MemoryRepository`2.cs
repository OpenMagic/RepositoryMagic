using System;
using System.Collections.Generic;

namespace RepositoryMagic
{
    public class MemoryRepository<TModel, TId> : Repository<TModel, TId>, IRepository<TModel, TId> where TModel : IModel<TId>
    {
        private static readonly Dictionary<TId, TModel> _Items = new Dictionary<TId, TModel>();

        public MemoryRepository()
        {
        }

        protected override void DeleteItem(TId id)
        {
            _Items.Remove(id);
        }

        protected override TModel FindItem(TId id)
        {
            TModel item;

            _Items.TryGetValue(id, out item);

            return item;
        }

        protected override IEnumerable<TModel> GetItems()
        {
            return _Items.Values;
        }

        protected override void InsertItem(TModel model)
        {
            _Items.Add(model.Id, model);
        }

        protected override bool ItemExists(TId id)
        {
            return _Items.ContainsKey(id);
        }

        protected override void UpdateItem(TModel model)
        {
            // nothing to do.
        }
    }
}
