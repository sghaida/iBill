using ORM.DataAttributes;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Helpers
{
   
    public static class DataReaderExtension
    {
        public static T ConvertToObject<T>(this OleDbDataReader dataReader) where T : class, new()
        {
            T dataObj = new T();

            List<PropertyInfo> masterPropertyInfoFields = new List<PropertyInfo>();
            Dictionary<string, List<ObjectPropertyInfoField>> cdtPropertyInfo = new Dictionary<string, List<ObjectPropertyInfoField>>();


            //List of T object data fields (DbColumnAttribute Values), and types.
            List<ObjectPropertyInfoField> masterObjectFields = new List<ObjectPropertyInfoField>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            // Initialize Master the property info fields list
            masterPropertyInfoFields = typeof(T).GetProperties(flags)
                .Where(property => property.GetCustomAttribute<DbColumnAttribute>() != null)
                .Cast<PropertyInfo>()
                .ToList();

            return dataObj;
        }
    }
}
