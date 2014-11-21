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
    public class DistributedDataAccess<T> : DataAccess<T>, IDistributedDataAccess<T> where T: class, new()
    {
        private DBLib DBRoutines = new DBLib();

        private string IDFieldName { set; get; }
        private List<DbTableField> DataProperties { set; get; }


        /// <summary>
        /// Tries to read the Class Db Properties, which are the properties marked with DbColumn Attribute. It tries to resolve the other attribute values, if they exist, 
        /// otherwise, it assigns the default values.
        /// </summary>
        /// <returns>List of DbTableProperty objects, if the class has DbColumn DataProperties.</returns>
        private List<DbTableField> tryReadClassDbProperties()
        {
            var objDataProperties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
                                                        Where(property => property.GetCustomAttribute<DbColumnAttribute>() != null).
                                                        ToList();

            if (objDataProperties != null && objDataProperties.Count() > 0)
            {
                return (
                    objDataProperties.Select(item => new DbTableField
                    {
                        ColumnName = item.GetCustomAttribute<DbColumnAttribute>().Name,
                        IsIDField = item.GetCustomAttribute<IsIDFieldAttribute>() != null ? item.GetCustomAttribute<IsIDFieldAttribute>().Status : false,
                        AllowNull = item.GetCustomAttribute<AllowNullAttribute>() != null ? item.GetCustomAttribute<AllowNullAttribute>().Status : false,
                        AllowIDInsert = item.GetCustomAttribute<AllowIDInsertAttribute>() != null ? item.GetCustomAttribute<AllowIDInsertAttribute>().Status : false,
                        FieldType = item.PropertyType
                    })
                        .ToList<DbTableField>()
                );
            }

            throw new Exception(String.Format("Couldn't find any class property marked with the [DbColumn] Attribute in the class \"{0}\". Kindly revise the class definition.", typeof(T).Name));
        }

        /// <summary>
        /// Tries to read the IDField property attribute value if it exists; if it doesn't it throws and exception
        /// </summary>
        /// <returns>IDField attribute value (string), if exists.</returns>
        private string tryReadIDFieldAttributeValue()
        {
            DbTableField IDField;

            //Get the IDField DbTableProperty attribute
            IDField = DataProperties.Find(item => item.IsIDField == true);

            if (IDField != null)
            {
                return IDField.ColumnName;
            }
            else
            {
                throw new Exception(String.Format("No ID field is defined. Kindly annotate the ID property in class \"{0}\" with the [IsIDField] Attribute.", typeof(T).Name));
            }
        }


        public DistributedDataAccess()
        {
            try
            {
                this.DataProperties = tryReadClassDbProperties();
                this.IDFieldName = tryReadIDFieldAttributeValue();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<string> GetTablesList()
        {
            throw new NotImplementedException();
        }

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
            DataTable dt;
            List<T> results = new List<T>();

            var tables = GetTablesList();

            foreach (var tableName in tables)
            {
                dt = DBRoutines.SELECT(tables.First(), null, null, 25);
                
                results = results.Concat(dt.ConvertToList<T>()).ToList<T>();
            }

            return results.AsQueryable<T>();
        }

        new public int Insert(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
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

        new public bool Delete(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
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

        new public bool Update(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
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

        new public T GetById(long id, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
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

        new public IQueryable<T> Get(Expression<Func<T, bool>> predicate, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
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

        new public IQueryable<T> Get(Dictionary<string, object> whereCondition, int limit = 25, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
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

        new public IQueryable<T> GetAll(string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default) 
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
