using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.Helpers;
using LyncBillingBase.DataAttributes;
using LyncBillingBase.Exceptions;


namespace LyncBillingBase.DataAccess
{
    public class DataAccess<T> : IDataAccess<T> where T : DataModel, new()
    {
        /**
         * Private instance variables
         */
        private DataSourceSchema<T> Schema;
        private static DBLib DBRoutines = new DBLib();

        /**
         * Repository Constructor
         */
        public DataAccess() 
        {
            //Get the Table Name and List of Class Attributes
            try
            {
                //Initialize the schema for the class T
                this.Schema = new DataSourceSchema<T>();

                //Check for absent or invalid DataModel attributes and throw the respective exception if they exist.
                if(string.IsNullOrEmpty(Schema.DataSourceName))
                {
                    throw new NoDataSourceNameException(typeof(T).Name);
                }
                else if(Schema.DataFields.Where(item => item.TableField != null).ToList().Count() == 0)
                {
                    throw new NoTableFieldsException(typeof(T).Name);
                }
                else if(string.IsNullOrEmpty(Schema.IDFieldName))
                {
                    throw new NoTableIDFieldException(typeof(T).Name);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public virtual int Insert(T dataObject, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();
          
            if (dataObject != null)
            {
                var properties = Schema.DataFields.Select(field => field.TableField).ToList();

                foreach (var property in properties)
                {
                    var dataObjectAttr = dataObject.GetType().GetProperty(property.ColumnName);

                    //Don't insert ID Fields into the Database
                    if(property.IsIDField == true)
                    {
                        continue;
                    }

                    //Continue handling the properties
                    if (property.AllowNull == false && dataObjectAttr != null)
                    {
                        var dataObjectAttrValue = dataObjectAttr.GetValue(dataObject, null);

                        if (dataObjectAttrValue != null)
                        {
                            columnsValues.Add(property.ColumnName, Convert.ChangeType(dataObjectAttrValue, property.FieldType));
                        }
                        else
                        {
                            throw new Exception("The Property " + property.ColumnName + " in the " + dataObject.GetType().Name + " Table is not allowed to be null kindly annotate the property with [IsAllowNull]");
                        }
                    }
                    else
                    {
                        var dataObjectAttrValue = dataObjectAttr.GetValue(dataObject, null);

                        if (dataObjectAttrValue != null)
                        {
                            columnsValues.Add(property.ColumnName, Convert.ChangeType(dataObjectAttrValue, property.FieldType));
                        }
                    }
                    //end-inner-if

                }//end-foreach

                try
                {
                    rowID = DBRoutines.INSERT(tableName: Schema.DataSourceName, columnsValues: columnsValues, idFieldName: Schema.IDFieldName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }//end-outer-if

            return rowID;  
        }


        public virtual bool Delete(T dataObject, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default)
        {
            long ID = 0;

            var dataObjectAttr = dataObject.GetType().GetProperty(Schema.IDFieldName);

            if (dataObjectAttr == null)
            {
                throw new Exception("There is no available ID field. kindly annotate " + typeof(T).Name);
            }
            else 
            {
                var dataObjectAttrValue = dataObjectAttr.GetValue(dataObject, null);

                if(dataObjectAttrValue == null)
                {
                    throw new Exception("There is no available ID field is presented but not set kindly set the value of the ID field Object for the following class: " + typeof(T).Name);
                }
                else
                {
                    long.TryParse(dataObjectAttrValue.ToString(),out ID);

                    return DBRoutines.DELETE(tableName: Schema.DataSourceName, idFieldName: Schema.IDFieldName, ID: ID);
                }//end-inner-if-else
            }//end-outer-if-else
        }


        public virtual bool Update(T dataObject, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default)
        {
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();
            bool status = false;

            if (dataObject != null)
            {
                var properties = Schema.DataFields.Select(field => field.TableField).ToList();

                foreach (var property in properties)
                {
                    var dataObjectAttr = dataObject.GetType().GetProperty(property.ColumnName);

                    //Don't insert ID Fields into the Database
                    if(property.IsIDField == true)
                    {
                        continue;
                    }

                    //Continue handling the properties
                    if (property.AllowNull == false && dataObjectAttr != null)
                    {
                        var dataObjectAttrValue = dataObjectAttr.GetValue(dataObject, null);

                        if (dataObjectAttrValue != null)
                        {
                            columnsValues.Add(property.ColumnName, Convert.ChangeType(dataObjectAttrValue, property.FieldType));
                        }
                        else
                        {
                            throw new Exception("The Property " + property.ColumnName + " in the " + dataObject.GetType().Name + " Table is not allowed to be null kindly annotate the property with [IsAllowNull]");
                        }
                    }
                    else
                    {
                        var dataObjectAttrValue = dataObjectAttr.GetValue(dataObject, null);

                        if (dataObjectAttrValue != null)
                        {
                            columnsValues.Add(property.ColumnName, Convert.ChangeType(dataObjectAttrValue, property.FieldType));
                        }
                    }
                    //end-inner-if

                }//end-foreach

                try
                {
                    status = DBRoutines.UPDATE(tableName: Schema.DataSourceName, columnsValues: columnsValues, wherePart: null);

                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }

            }//end-outer-if

            return status;  
        }


        public virtual T GetById(long id, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default)
        {
            string errorMessage = string.Empty;

            if (id > 0)
            {
                DataTable dt =  DBRoutines.SELECT(Schema.DataSourceName, Schema.IDFieldName, id);

                if (dt.Rows.Count == 0)
                {
                    return (T)Activator.CreateInstance(typeof(T));
                }
                else 
                {
                    return dt.ConvertToList<T>().FirstOrDefault<T>()??null; 
                }
            }

            errorMessage = String.Format("The ID Field is either null or zero. Kindly pass a valid ID. Class name: \"{0}\".", typeof(T).Name);

            throw new Exception(errorMessage);
        }


        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> predicate, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default)
        {
            DataTable dt = new DataTable();

            if (predicate == null) 
            {
                var errorMessage = string.Format("There is no defined Predicate. {0} ",typeof(T).Name);
                
                throw new Exception(errorMessage);
            }
            else 
            {
                CustomExpressionVisitor ev = new CustomExpressionVisitor();
                
                string whereClause = ev.Translate(predicate);

                if (string.IsNullOrEmpty(dataSourceName))
                {

                    if (string.IsNullOrEmpty(whereClause))
                    {
                        dt = DBRoutines.SELECT(Schema.DataSourceName);
                    }
                    else
                    {
                        dt = DBRoutines.SELECT(Schema.DataSourceName, whereClause);
                    }
                }
                else 
                {
                    if (string.IsNullOrEmpty(whereClause))
                    {
                        dt = DBRoutines.SELECT(dataSourceName);
                    }
                    else
                    {
                        dt = DBRoutines.SELECT(dataSourceName, whereClause);
                    }
                }
            }

            return dt.ConvertToList<T>();
        }


        public virtual IEnumerable<T> Get(Dictionary<string, object> whereCondition, int limit = 25, string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default)
        {
            string errorMessage = string.Empty;
            List<string> allColumns = null;

            if (whereCondition != null && whereCondition.Count > 0)
            {
                DataTable dt = DBRoutines.SELECT(Schema.DataSourceName, allColumns, whereCondition, limit);

                return dt.ConvertToList<T>();
            }

            errorMessage = String.Format("The \"whereConditions\" parameter is either null or empty. Kindly pass a valid \"whereConditions\" parameter. Class name: \"{0}\".", typeof(T).Name);

            throw new Exception(errorMessage);
        }


        public virtual IEnumerable<T> GetAll(string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default)
        {
            int maximumLimit = 0;
            List<string> allColumns = null;
            Dictionary<string, object> whereConditions = null;
            
            DataTable dt  = new DataTable();

            if (Schema.DataSourceType == Enums.DataSourceType.DBTable)
            {
                if (string.IsNullOrEmpty(dataSourceName))
                {
                    dt = DBRoutines.SELECT(Schema.DataSourceName, allColumns, whereConditions, maximumLimit);
                }
                else
                {
                    dt = DBRoutines.SELECT(dataSourceName, allColumns, whereConditions, maximumLimit);
                }
            }

            return dt.ConvertToList<T>();
        }


        public virtual IEnumerable<T> GetAllWithRelations(string dataSourceName = null, Enums.DataSourceType dataSource = Enums.DataSourceType.Default)
        {
            int maximumLimit = 0;
            List<string> allColumns = null;
            Dictionary<string, object> whereConditions = null;

            string sql = string.Empty;
            Dictionary<string, Dictionary<string, string>> tableRelationsMap = new Dictionary<string, Dictionary<string, string>>();

            DataTable dt = new DataTable();

            var tableRelations = Schema.DataFields.Where(field => field.Relation != null).Select<DataField, DbRelation>(field => field.Relation).ToList<DbRelation>();

            if (tableRelations.Count() > 0)
            {
                foreach(var relation in tableRelations)
                {
                    Type relationType = relation.WithDataModel;
                    var generalModelSchemaType = typeof(DataSourceSchema<>);
                    var specialModelSchemaType = generalModelSchemaType.MakeGenericType(relationType);

                    var targetTableSchema = Activator.CreateInstance(specialModelSchemaType);
                }
            }

            if (Schema.DataSourceType == Enums.DataSourceType.DBTable)
            {
                if (string.IsNullOrEmpty(dataSourceName))
                {
                    dt = DBRoutines.SELECT(Schema.DataSourceName, allColumns, whereConditions, maximumLimit);
                }
                else
                {
                    dt = DBRoutines.SELECT(dataSourceName, allColumns, whereConditions, maximumLimit);
                }
            }

            return dt.ConvertToList<T>();

        }


        public virtual IEnumerable<T> GetAll(string sql)
        {
            DataTable dt = DBRoutines.SELECTFROMSQL(sql);

            return dt.ConvertToList<T>();
        }


        public virtual int Insert(string sql)
        {
            int id = DBRoutines.INSERT(sql);
            
            return id;
        }


        public virtual bool Update(string sql)
        {
            bool status = DBRoutines.UPDATE(sql);

            return status;
        }


        public virtual bool Delete(string sql)
        {
            bool status = DBRoutines.DELETE(sql);

            return status;
        }

    }

      
}
