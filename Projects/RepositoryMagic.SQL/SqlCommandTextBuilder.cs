using System.Collections.Generic;
using System.Linq;

namespace RepositoryMagic.SQL
{
    public class SqlCommandTextBuilder<TModel, TId> : ISqlCommandTextBuilder<TModel, TId> where TModel : IModel<TId>
    {
        public string GetItems()
        {
            var columnNames = this.QuotedColumnNames();
            var tableName = this.QuotedTableName();

            return string.Format("SELECT {0} FROM {1};", columnNames, tableName);
        }

        public string InsertItem()
        {
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2});", this.QuotedTableName(), this.QuotedColumnNames(), this.GetParameterNames());
        }

        public string DeleteItem()
        {
            return string.Format("DELETE FROM {0} WHERE {1} = @Id;", this.QuotedTableName(), this.QuotedPrimaryColumn());
        }

        public string FindItem()
        {
            return string.Format("SELECT {0} FROM {1} WHERE {2} = @Id;", this.QuotedColumnNames(), this.QuotedTableName(), this.QuotedPrimaryColumn());
        }

        private IEnumerable<string> GetColumnNames()
        {
            return from propertyInfo in typeof(TModel).GetProperties()
                   select propertyInfo.Name;
        }

        private string GetParameterNames()
        {
            var parameterNames = from columnName in this.GetColumnNames()
                                 select this.ParameterName(columnName);

            return string.Join(", ", parameterNames.ToArray());
        }

        public string ItemExists()
        {
            return string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = @Id", this.QuotedTableName(), this.QuotedPrimaryColumn());
        }

        private string ParameterName(string columnName)
        {
            return "@" + columnName;
        }

        public string UpdateItem()
        {
            var setColumns = from columnName in this.GetColumnNames().Skip(1)
                             select string.Format("{0} = {1}", this.Quote(columnName), this.ParameterName(columnName));
                             
                             
            return string.Format("UPDATE {0} SET {1} WHERE {2} = @Id", this.QuotedTableName(), string.Join(", ", setColumns.ToArray()), this.QuotedPrimaryColumn());
        }

        private string Quote(string value)
        {
            return string.Format("[{0}]", value);
        }

        private string QuotedColumnNames()
        {
            var columnNames = from columnName in this.GetColumnNames()
                              select this.Quote(columnName);

            return string.Join(", ", columnNames.ToArray());
        }

        private string QuotedPrimaryColumn()
        {
            return this.Quote(this.GetColumnNames().First());
        }

        private string QuotedTableName()
        {
            // todo: pluralize
            return this.Quote(typeof(TModel).Name + "s");
        }
    }
}
