using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data;
using System.Data.Linq;


namespace LyncBillingBase.Repository
{
   
    public class Repository<T> : IRepository<T>
    {
        public class DBTableProperty
        {
            public string Name { get; set; }
            public bool IsAllowNull { get; set; }
            public bool IsIDField { get; set; }
            public Type FieldType { get; set; }
        }

        private static DBLib DBRoutines = new DBLib();

        public string TableName { private set; get; }
        
        public string IDField { private set; get; }
        
        public List<DBTableProperty> Properties { private set; get; }
        
        public Repository() 
        {
            //Get the Table Name
            var tableName = typeof(T).GetCustomAttributes(typeof(TableNameAttribute));

            if (tableName == null || tableName.Count() == 0)
            {
                throw new Exception("Table name was not provided for " + typeof(T).GetType().Name);
            }
            else 
            {
                this.TableName = ((TableNameAttribute)tableName.First()).Description;
            }

            //Get the list of instance variables
            var objProperties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (objProperties == null || objProperties.Count() == 0)
            {
                throw new Exception("Cant find valid properties in " + typeof(T).GetType().Name);
            }
            else
            {
                //Get Properties names and if ISNullIsFlase
                Properties = objProperties.Select(item =>
                    new DBTableProperty
                    {
                        Name = item.Name,
                        IsAllowNull = item.GetCustomAttribute<IsAllowNullAttribute>() != null ? true : false,
                        IsIDField = item.GetCustomAttribute<IsIdFieldAttribute>() != null ? true : false,
                        FieldType = item.PropertyType
                    }).ToList<DBTableProperty>();

            }

            var IDField = Properties.Find(item => item.IsIDField == true);

            if (IDField == null)
            {
                throw new Exception("No ID field is defined. kindly annotitate " + typeof(T).GetType().Name + " ID property with [IsIdField]");
            }
            else 
            {
                this.IDField = IDField.Name;
            }
        } 

        public int Insert(T dataObject)
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>();
          
            if (dataObject != null)
            {
                foreach (var property in Properties)
                {
                    var dataObjectAttr = dataObject.GetType().GetProperty(property.Name);

                    //Don't insert ID Fields into the Database
                    if(property.IsIDField == true)
                    {
                        continue;
                    }

                    //Continue handling the properties
                    if (property.IsAllowNull == false && dataObjectAttr != null)
                    {
                        var dataObjectAttrValue = dataObjectAttr.GetValue(dataObject, null);

                        if (dataObjectAttrValue != null)
                        {
                            columnsValues.Add(property.Name, Convert.ChangeType(dataObjectAttrValue, property.FieldType));
                        }
                        else
                        {
                            throw new Exception("The Property " + property.Name + " in the " + dataObject.GetType().Name + " Table is not allowed to be null kindly annotate the property with [IsAllowNull]");
                        }
                    }
                    else
                    {
                        var dataObjectAttrValue = dataObjectAttr.GetValue(dataObject, null);

                        if (dataObjectAttrValue != null)
                        {
                            columnsValues.Add(property.Name, Convert.ChangeType(dataObjectAttrValue, property.FieldType));
                        }
                    }
                    //end-inner-if

                }//end-foreach

                try
                {
                  
                   
                    rowID = DBRoutines.INSERT(tableName: TableName, columnsValues: columnsValues, idFieldName: IDField);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }//end-outer-if

            return rowID;  
        }

        public bool Delete(T dataObject)
        {
            long ID= 0;

            var dataObjectAttr = dataObject.GetType().GetProperty(IDField);

            if (dataObjectAttr == null)
            {
                throw new Exception("There is no available ID field. kindly annotate " + dataObject.GetType().Name);
            }
            else 
            {
                var dataObjectAttrValue = dataObjectAttr.GetValue(dataObject, null);

                if(dataObjectAttrValue == null)
                {
                    throw new Exception("There is no available ID field is presented but not set kindly set the value of the ID field Object for the following class: " + dataObject.GetType().Name);
                }
                else
                {
                    
                    long.TryParse(dataObjectAttrValue.ToString(),out ID);

                    return DBRoutines.DELETE(tableName: TableName, idFieldName: IDField, ID: ID);
                }
                
            }

        }


        public bool Update(T dataObject)
        {
            throw new NotImplementedException();
        }

      
        public T GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(Dictionary<string, object> where, int limit = 25)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
