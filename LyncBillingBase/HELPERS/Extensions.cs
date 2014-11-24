using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.Libs;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.Helpers
{
    // Helper class for the ConverToList<T> function
    public class ObjectPropertyInfoField
    {
        public PropertyInfo Property { get; set; }
        public string DataFieldName { get; set; }
        public Type DataFieldType { get; set; }
    }


    public static class Extensions
    {
        /// <summary>
        /// Converts datatable to list<T> dynamically
        /// </summary>
        /// <typeparam name="T">Class name</typeparam>
        /// <param name="dataTable">data table to convert</param>
        /// <returns>List<T></returns>
        public static List<T> ConvertToList<T>(this DataTable dataTable) where T : class ,new()
        {
            var dataList = new List<T>();
            
            // List of class property infos
            List<PropertyInfo> propertyInfoFields = new List<PropertyInfo>();

            //List of T object data fields (DbColumnAttribute Values), and types.
            List<ObjectPropertyInfoField> objectFields = new List<ObjectPropertyInfoField>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            // Initialize the property info fields list
            propertyInfoFields = typeof(T).GetProperties(flags)
                .Where(property => property.GetCustomAttribute<DbColumnAttribute>() != null)
                .Cast<PropertyInfo>()
                .ToList();
            
            // Initialize the object data fields  list
            foreach(var item in propertyInfoFields)
            {
                objectFields.Add(new ObjectPropertyInfoField {
                    Property = item,
                    DataFieldName = item.GetCustomAttribute<DbColumnAttribute>().Name,
                    DataFieldType = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                });
            }

            //Read Datatable column names and types
            var dtlFieldNames = dataTable.Columns.Cast<DataColumn>()
                .Select(item => new { 
                    Name = item.ColumnName, 
                    Type = item.DataType 
                }).ToList();

            //Begin data table processing
            Parallel.ForEach(dataTable.AsEnumerable().ToList(),
                (
                    datarow) =>
                    {
                        var classObj = new T();

                        foreach (var dtField in dtlFieldNames)
                        {
                            var dataField = objectFields.Find(item => item.DataFieldName == dtField.Name);

                            if (dataField != null)
                            {
                                // Get the property info object of this field, for easier accessibility
                                PropertyInfo dataFieldPropertyInfo = dataField.Property;

                                if (dataFieldPropertyInfo.PropertyType == typeof(DateTime))
                                {
                                    dataFieldPropertyInfo.SetValue(classObj, datarow[dtField.Name].ReturnDateTimeMinIfNull(), null);
                                }
                                else if (dataFieldPropertyInfo.PropertyType == typeof(int))
                                {
                                    dataFieldPropertyInfo.SetValue(classObj, datarow[dtField.Name].ReturnZeroIfNull(), null);
                                }
                                else if (dataFieldPropertyInfo.PropertyType == typeof(long))
                                {
                                    dataFieldPropertyInfo.SetValue(classObj, datarow[dtField.Name].ReturnZeroIfNull(), null);
                                }
                                else if (dataFieldPropertyInfo.PropertyType == typeof(decimal))
                                {
                                    dataFieldPropertyInfo.SetValue(classObj, datarow[dtField.Name].ReturnZeroIfNull(), null);
                                }
                                else if (dataFieldPropertyInfo.PropertyType == typeof(String))
                                {
                                    if (datarow[dtField.Name].GetType() == typeof(DateTime))
                                    {
                                        dataFieldPropertyInfo.SetValue(classObj, ConvertToDateString(datarow[dtField.Name]), null);
                                    }
                                    else
                                    {
                                        dataFieldPropertyInfo.SetValue(classObj, datarow[dtField.Name].ReturnEmptyIfNull(), null);
                                    }
                                }
                            }
                        }
                        dataList.Add(classObj);
                });
           
            return dataList;
        }

        private static string ConvertToDateString(object date) 
        {
            if (date == null)
                return string.Empty;
           
            return Convert.ToDateTime(date).ConvertDate();
        }

    }
}
