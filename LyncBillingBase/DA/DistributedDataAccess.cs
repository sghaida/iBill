using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.Helpers;
using LyncBillingBase.Libs;


namespace LyncBillingBase.DA
{
    public class DistributedDataAccess<T> : IDataAccess<T> where T : class, new()
    {
        public virtual List<String> GetListOfTables()
        {
            throw new NotImplementedException();
        }

        public int Insert(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default)
        {
            throw new NotImplementedException();
        }

        public bool Update(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default)
        {
            throw new NotImplementedException();
        }

        public T GetById(long id, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(Dictionary<string, object> where, int limit = 25, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll(string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default)
        {
            var tables = this.GetListOfTables();
            DataTable dt = new DataTable();

            foreach (var table in tables)
            {
                string x = string.Empty;
            }

            return (new DataTable().ConvertToList<T>()).AsQueryable<T>();
        }
    }
}
