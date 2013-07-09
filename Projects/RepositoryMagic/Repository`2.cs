using System;
using System.Collections.Generic;

namespace RepositoryMagic
{
    public abstract class Repository<TModel, TId> : IRepository<TModel, TId> where TModel : IModel<TId>
    {
        public virtual void Delete(TId id)
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

        public virtual bool Exists(TId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            return this.ItemExists(id);
        }

        public virtual TModel Find(TId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            return this.FindItem(id);
        }

        abstract protected TModel FindItem(TId id);

        public virtual TModel Get(TId id)
        {
            var item = this.Find(id);

            if (item != null)
            {
                return item;
            }

            throw new ItemNotFoundException(string.Format("{0} does not exist.", this.ModelId(id)));
        }

        public virtual IEnumerable<TModel> Get()
        {
            return this.GetItems();
        }

        abstract protected IEnumerable<TModel> GetItems();

        public virtual void Insert(TModel model)
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

        public virtual string ModelId(TId id)
        {
            return string.Format("{0}/{1}", typeof(TModel), id);
        }

        public virtual void Update(TModel model)
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
