using System;
using System.Collections.Generic;

namespace RepositoryMagic
{
    public abstract class Repository<TModel, TId> : IRepository<TModel, TId> where TModel : IModel<TId>
    {
        public void Delete(TId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (!this.Exists(id))
            {
                throw new ItemNotFoundException(String.Format("{0} cannot be deleted because it does not exist.", this.ModelId(id)));
            }

            this.DeleteItem(id);
        }

        abstract protected void DeleteItem(TId id);

        public bool Exists(TId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            return this.ItemExists(id);
        }

        public TModel Find(TId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            return this.FindItem(id);
        }

        abstract protected TModel FindItem(TId id);

        public TModel Get(TId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            return this.GetItem(id);
        }

        public IEnumerable<TModel> Get()
        {
            return this.GetItems();
        }

        abstract protected TModel GetItem(TId id);

        abstract protected IEnumerable<TModel> GetItems();

        public void Insert(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (this.Exists(model.Id))
            {
                throw new DuplicateItemException(String.Format("{0} cannot be inserted because it already exists.", this.ModelId(model.Id)));
            }
            this.InsertItem(model);
        }

        abstract protected void InsertItem(TModel model);

        abstract protected bool ItemExists(TId id);

        public string ModelId(TId id)
        {
            return string.Format("{0}/{1}", typeof(TModel), id);
        }

        public void Update(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (!this.Exists(model.Id))
            {
                throw new ItemNotFoundException(String.Format("{0} cannot be updated because it does not exist.", this.ModelId(model.Id)));
            }

            this.UpdateItem(model);
        }

        abstract protected void UpdateItem(TModel model);
    }
}
