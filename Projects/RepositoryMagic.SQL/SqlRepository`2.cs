using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using OpenMagic;

namespace RepositoryMagic.SQL
{
    public class SqlRepository<TModel, TId> : CachedRepository<TModel, TId>, IRepository<TModel, TId> where TModel : IModel<TId>
    {
        private IDbTransaction _Transaction;
        private ISqlCommandTextBuilder<TModel, TId> _CommandTextBuilder;

        public SqlRepository(IDbTransaction transaction)
        {
            transaction.MustNotBeNull("transaction");

            _Transaction = transaction;
            _CommandTextBuilder = new SqlCommandTextBuilder<TModel, TId>();
        }

        protected override void DeleteItem(TId id)
        {
            string sql = _CommandTextBuilder.DeleteItem();

            this.Execute(sql, new { Id = id });
        }

        private void Execute(string sql, object param)
        {
            _Transaction.Connection.Execute(sql, param, _Transaction);
        }

        protected override TModel FindItem(TId id)
        {
            string sql = _CommandTextBuilder.FindItem();

            return this.Query(sql, new { Id = id }).SingleOrDefault();
        }

        protected override IEnumerable<TModel> GetItems()
        {
            string sql = _CommandTextBuilder.GetItems();

            return this.Query(sql, null);
        }

        protected override void InsertItem(TModel model)
        {
            string sql = _CommandTextBuilder.InsertItem();

            _Transaction.Connection.Execute(sql, model, _Transaction);
        }

        protected override bool ItemExists(TId id)
        {
            string sql = _CommandTextBuilder.ItemExists();

            return (_Transaction.Connection.Query<long>(sql, new { Id = id }, _Transaction).Single() == 1);
        }

        private IDbCommand NewCommand(string commandText)
        {
            return _Transaction.Connection.CreateCommand();
        }

        private IEnumerable<TModel> Query(string sql, object param)
        {
            return _Transaction.Connection.Query<TModel>(sql, param, _Transaction);
        }

        protected override void UpdateItem(TModel model)
        {
            string sql = _CommandTextBuilder.UpdateItem();

            this.Execute(sql, model);
        }

    }
}
