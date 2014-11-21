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
    public class DistributedDataAccess<T> :DataAccess<T>, IDistributedDataAccess<T> where T:class, new()
    {
        
        public int Insert(string SQL)
        {
            throw new NotImplementedException();
        }

        public bool Update(string SQL)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public DistributedDataAccess() 
        {
            
        }

        int Insert(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
        {
            if (dataSourceName == null && dataSource != Enums.DataSources.Default)
            {
                return base.Insert(dataObject, dataSourceName, dataSource);
            }
            else
            {
                return  base.Insert(dataObject);
            }
        }

        public bool Delete(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
        {
            if (dataSourceName == null && dataSource != Enums.DataSources.Default)
            {
                return base.Delete(dataObject, dataSourceName, dataSource);
            }
            else
            {
                return base.Delete(dataObject);
            }
        }

        public bool Update(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
        {
            if (dataSourceName == null && dataSource != Enums.DataSources.Default)
            {
                return base.Update(dataObject, dataSourceName, dataSource);
            }
            else
            {
                return base.Update(dataObject);
            }
        }

        public T GetById(long id, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
        {
            if (dataSourceName == null && dataSource != Enums.DataSources.Default)
            {
                return base.GetById(id, dataSourceName, dataSource);
            }
            else
            {
                return base.GetById(id);
            }
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
        {
            if (dataSourceName == null && dataSource != Enums.DataSources.Default)
            {
                return base.Get(predicate, dataSourceName, dataSource);
            }
            else
            {
                return base.Get(predicate);
            }
        }

        public IQueryable<T> Get(Dictionary<string, object> whereCondition, int limit = 25, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
        {
            if (dataSourceName == null && dataSource != Enums.DataSources.Default)
            {
                return base.Get(whereCondition, limit=25, dataSourceName, dataSource);
            }
            else
            {
                return base.Get(whereCondition,limit=25);
            }
        }

        public IQueryable<T> GetAll(string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
        {
            if (dataSourceName == null && dataSource != Enums.DataSources.Default)
            {
                return base.GetAll(dataSourceName, dataSource);
            }
            else
            {
                return base.GetAll();
            }
        }
    } 
}
