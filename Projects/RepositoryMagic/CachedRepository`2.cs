using System;
using System.Collections.Generic;
using System.Linq;
using OpenMagic;

namespace RepositoryMagic
{
    public abstract class CachedRepository<TModel, TId> : 
        Repository<TModel, TId>, 
        IRepository<TModel, TId>, 
        IDisposable 
        where TModel : IModel<TId>
    {
        private Dictionary<TId, TModel> _Cache = new Dictionary<TId, TModel>();

        public override void Delete(TId id)
        {
            base.Delete(id);
            _Cache.Remove(id);
        }

        public void Dispose()
        {
            _Cache.Clear();
        }

        public override bool Exists(TId id)
        {
            id.MustNotBeNull("id");

            if (_Cache.ContainsKey(id))
            {
                return true;
            }

            return base.Exists(id);
        }

        public override TModel Find(TId id)
        {
            id.MustNotBeNull("id");

            TModel model;

            if (_Cache.TryGetValue(id, out model))
            {
                return model;
            }

            return base.Find(id);
        }

        public override IEnumerable<TModel> Get()
        {
            var newItems = from i in base.Get()
                           where !_Cache.ContainsKey(i.Id)
                           select i;

            foreach (var item in newItems)
            {
                _Cache.Add(item.Id, item);
            }

            return _Cache.Values;
        }

        public override void Insert(TModel model)
        {
            base.Insert(model);

            // We must test if Id has been set because some repositories will not set the id until after a commit.
            if (model.Id != null)
            {
                _Cache.Add(model.Id, model);
            }
        }
    }
}
