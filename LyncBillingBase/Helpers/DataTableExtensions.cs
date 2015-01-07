using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.Libs;
using LyncBillingBase.DataAttributes;
using System.ComponentModel;
using System.Linq.Expressions;
using LyncBillingBase.DataAccess;
using System.Data.SqlTypes;

namespace LyncBillingBase.Helpers
{
    public static class DataTableExtentions
    {
        // 
        // Helper function
        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return Convert.ToDateTime(date).ConvertDate();
        }//end-ConvertToDateString-function


        [Obsolete]
        public static List<T> ConvertToList_OLD<T>(this DataTable DataTable) where T : class, new()
        {
            var dataList = new List<T>();

            

            // List of class property infos
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

            //Read Datatable column names and types
            var dtlFieldNames = DataTable.Columns.Cast<DataColumn>()
                .Select(item => new
                {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

            // Initialize the object data fields  list for Master Object
            foreach (var item in masterPropertyInfoFields)
            {
                masterObjectFields.Add(new ObjectPropertyInfoField
                {
                    Property = item,
                    DataFieldName = item.GetCustomAttribute<DbColumnAttribute>().Name,
                    DataFieldType = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                });
            }


            //Fill The data
            //foreach (var datarow in DataTable.AsEnumerable().ToList())   
            //{
            Parallel.ForEach(DataTable.AsEnumerable().ToList(),
                 (datarow) =>
                 {
                     var masterObj = new T();

                     //Fill master Object with its related properties values
                     foreach (var dtField in masterObjectFields)
                     {
                         if (dtField != null)
                         {
                             // Get the property info object of this field, for easier accessibility
                             PropertyInfo dataFieldPropertyInfo = dtField.Property;

                             if (dataFieldPropertyInfo.PropertyType == typeof(DateTime))
                             {
                                 dataFieldPropertyInfo.SetValue(masterObj, datarow[dtField.DataFieldName].ReturnDateTimeMinIfNull(), null);
                             }
                             else if (dataFieldPropertyInfo.PropertyType == typeof(int))
                             {
                                 dataFieldPropertyInfo.SetValue(masterObj, datarow[dtField.DataFieldName].ReturnZeroIfNull(), null);
                             }
                             else if (dataFieldPropertyInfo.PropertyType == typeof(long))
                             {
                                 dataFieldPropertyInfo.SetValue(masterObj, datarow[dtField.DataFieldName].ReturnZeroIfNull(), null);
                             }
                             else if (dataFieldPropertyInfo.PropertyType == typeof(decimal))
                             {
                                 dataFieldPropertyInfo.SetValue(masterObj, Convert.ToDecimal(datarow[dtField.DataFieldName].ReturnZeroIfNull()), null);
                             }
                             else if (dataFieldPropertyInfo.PropertyType == typeof(String))
                             {
                                 if (datarow[dtField.DataFieldName].GetType() == typeof(DateTime))
                                 {
                                     dataFieldPropertyInfo.SetValue(masterObj, ConvertToDateString(datarow[dtField.DataFieldName]), null);
                                 }
                                 else
                                 {
                                     dataFieldPropertyInfo.SetValue(masterObj, datarow[dtField.DataFieldName].ReturnEmptyIfNull(), null);
                                 }
                             }
                         }//end if
                     }//end foreach

                     lock (dataList)
                     {
                         dataList.Add(masterObj);
                     }
                 });
            //}

            return dataList;
        }


        [Obsolete]
        public static List<T> ConvertToList_OLD<T>(this DataTable DataTable, params Expression<Func<T, object>>[] path) where T : class, new()
        {
            var dataList = new List<T>();

            // List of class property infos
            List<PropertyInfo> masterPropertyInfoFields = new List<PropertyInfo>();
            List<PropertyInfo> childPropertInfoFields = new List<PropertyInfo>();

            Dictionary<string, List<ObjectPropertyInfoField>> childrenObjectsProperties = new Dictionary<string, List<ObjectPropertyInfoField>>();
            Dictionary<string, List<ObjectPropertyInfoField>> cdtPropertyInfo = new Dictionary<string, List<ObjectPropertyInfoField>>();

            //List of T object data fields (DbColumnAttribute Values), and types.
            List<ObjectPropertyInfoField> masterObjectFields = new List<ObjectPropertyInfoField>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            Dictionary<string, string> expressionLookup = new Dictionary<string, string>();

            foreach (var t in path)
            {
                expressionLookup.Add((t.Body as MemberExpression).Member.Name, t.Body.Type.Name);
            }

            // Initialize Master the property info fields list
            masterPropertyInfoFields = typeof(T).GetProperties(flags)
                .Where(property => property.GetCustomAttribute<DbColumnAttribute>() != null)
                .Cast<PropertyInfo>()
                .ToList();

            //Read Datatable column names and types
            var dtlFieldNames = DataTable.Columns.Cast<DataColumn>()
                .Select(item => new
                {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

            // Initialize the object data fields  list for Master Object
            foreach (var item in masterPropertyInfoFields)
            {
                masterObjectFields.Add(new ObjectPropertyInfoField
                {
                    Property = item,
                    DataFieldName = item.GetCustomAttribute<DbColumnAttribute>().Name,
                    DataFieldType = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                });
            }


            if (path.Count() > 0)
            {
                // Initialize child the property info fields list
                childPropertInfoFields = typeof(T).GetProperties(flags)
                    .Where(property => property.GetCustomAttribute<DataRelationAttribute>() != null && expressionLookup.Keys.Contains(property.Name))
                    .Cast<PropertyInfo>()
                    .ToList();

                // Fill the childrenObjectsProperties dictionary with the name of the children class for reflection and their corrospndant attributes
                foreach (PropertyInfo property in childPropertInfoFields)
                {
                    Type childtypedObject = property.PropertyType;

                    var childtableFields = childtypedObject.GetProperties(flags)
                          .Where(item => item.GetCustomAttribute<DbColumnAttribute>() != null)
                          .Select(item => new ObjectPropertyInfoField
                          {
                              Property = (PropertyInfo)item,
                              DataFieldName = item.GetCustomAttribute<DbColumnAttribute>().Name,
                              DataFieldType = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                          })
                          .ToList();

                    var tableName = property.GetCustomAttribute<DataRelationAttribute>().Name;
                    childrenObjectsProperties.Add(tableName, childtableFields);
                }

                //Get the Children classes related columns from datatable
                foreach (KeyValuePair<string, List<ObjectPropertyInfoField>> childObjectsProperties in childrenObjectsProperties)
                {
                    var childObjectColumns = (from childObjField in childObjectsProperties.Value
                                              join dtlFieldName in dtlFieldNames on
                                              (childObjectsProperties.Key + "." + childObjField.DataFieldName) equals dtlFieldName.Name
                                              where dtlFieldName.Type == childObjField.DataFieldType
                                              select
                                              new ObjectPropertyInfoField()
                                              {
                                                  DataFieldName = dtlFieldName.Name,
                                                  DataFieldType = dtlFieldName.Type,
                                                  Property = childObjField.Property,
                                                  ObjectFieldName = childObjField.DataFieldName
                                              }).ToList();

                    if (childObjectColumns.Count > 0)
                    {
                        cdtPropertyInfo.Add(childObjectsProperties.Key, childObjectColumns);
                    }
                }
            }

            //Fill The data
            //foreach (var datarow in DataTable.AsEnumerable().ToList())   
            //{
            Parallel.ForEach(DataTable.AsEnumerable().ToList(),
                 (datarow) =>
                 {
                     var masterObj = new T();

                     if (path.Count() > 0)
                     {
                         //Fill the Data for children objects
                         foreach (PropertyInfo property in childPropertInfoFields)
                         {
                             List<ObjectPropertyInfoField> data;
                             cdtPropertyInfo.TryGetValue(property.GetCustomAttribute<DataRelationAttribute>().Name, out data);

                             // In order not to instantiate unwanted objects
                             if (data != null)
                             {
                                 Type childtypedObject = property.PropertyType;
                                 var childObj = Activator.CreateInstance(childtypedObject);

                                 foreach (var dtField in data)
                                 {
                                     var dataField = data.Find(item => item.DataFieldName == dtField.DataFieldName);

                                     if (dataField != null)
                                     {
                                         PropertyInfo dataFieldPropertyInfo = dataField.Property;

                                         if (dataFieldPropertyInfo.PropertyType == typeof(DateTime))
                                         {
                                             dataFieldPropertyInfo.SetValue(childObj, datarow[dtField.DataFieldName].ReturnDateTimeMinIfNull(), null);
                                         }
                                         else if (dataFieldPropertyInfo.PropertyType == typeof(int))
                                         {
                                             dataFieldPropertyInfo.SetValue(childObj, datarow[dtField.DataFieldName].ReturnZeroIfNull(), null);
                                         }
                                         else if (dataFieldPropertyInfo.PropertyType == typeof(long))
                                         {
                                             dataFieldPropertyInfo.SetValue(childObj, datarow[dtField.DataFieldName].ReturnZeroIfNull(), null);
                                         }
                                         else if (dataFieldPropertyInfo.PropertyType == typeof(decimal))
                                         {
                                             dataFieldPropertyInfo.SetValue(childObj, Convert.ToDecimal(datarow[dtField.DataFieldName].ReturnZeroIfNull()), null);
                                         }
                                         else if (dataFieldPropertyInfo.PropertyType == typeof(Char))
                                         {
                                             dataFieldPropertyInfo.SetValue(childObj, Convert.ToString(datarow[dtField.DataFieldName].ReturnEmptyIfNull()), null);
                                         }
                                         else if (dataFieldPropertyInfo.PropertyType == typeof(String))
                                         {
                                             if (datarow[dtField.DataFieldName].GetType() == typeof(DateTime))
                                             {
                                                 dataFieldPropertyInfo.SetValue(childObj, ConvertToDateString(datarow[dtField.DataFieldName]), null);
                                             }
                                             else
                                             {
                                                 dataFieldPropertyInfo.SetValue(childObj, datarow[dtField.DataFieldName].ReturnEmptyIfNull(), null);
                                             }
                                         }
                                     }
                                 }


                                 //Set the values for the children object
                                 foreach (PropertyInfo masterPropertyInfo in childPropertInfoFields)
                                 {
                                     if (masterPropertyInfo.PropertyType.Name == childObj.GetType().Name)
                                     {
                                         masterPropertyInfo.SetValue(masterObj, childObj);
                                     }
                                 }
                             }

                         }// end foreach
                     }
                     //Fill master Object with its related properties values
                     foreach (var dtField in masterObjectFields)
                     {

                         if (dtField != null)
                         {
                             // Get the property info object of this field, for easier accessibility
                             PropertyInfo dataFieldPropertyInfo = dtField.Property;

                             if (dataFieldPropertyInfo.PropertyType == typeof(DateTime))
                             {
                                 dataFieldPropertyInfo.SetValue(masterObj, datarow[dtField.DataFieldName].ReturnDateTimeMinIfNull(), null);
                             }
                             else if (dataFieldPropertyInfo.PropertyType == typeof(int))
                             {
                                 dataFieldPropertyInfo.SetValue(masterObj, datarow[dtField.DataFieldName].ReturnZeroIfNull(), null);
                             }
                             else if (dataFieldPropertyInfo.PropertyType == typeof(long))
                             {
                                 dataFieldPropertyInfo.SetValue(masterObj, datarow[dtField.DataFieldName].ReturnZeroIfNull(), null);
                             }
                             else if (dataFieldPropertyInfo.PropertyType == typeof(decimal))
                             {
                                 dataFieldPropertyInfo.SetValue(masterObj, Convert.ToDecimal(datarow[dtField.DataFieldName].ReturnZeroIfNull()), null);
                             }
                             else if (dataFieldPropertyInfo.PropertyType == typeof(String))
                             {
                                 if (datarow[dtField.DataFieldName].GetType() == typeof(DateTime))
                                 {
                                     dataFieldPropertyInfo.SetValue(masterObj, ConvertToDateString(datarow[dtField.DataFieldName]), null);
                                 }
                                 else
                                 {
                                     dataFieldPropertyInfo.SetValue(masterObj, datarow[dtField.DataFieldName].ReturnEmptyIfNull(), null);
                                 }
                             }
                         }//end if
                     }//end foreach

                     lock (dataList)
                     {
                         dataList.Add(masterObj);
                     }
                 });
            //}

            return dataList;
        }


        public static List<T> ConvertToList<T>(this DataTable DataTable) where T : class,new()
        {
            List<T> dataList = new List<T>();

            Dictionary<string, Action<T, object>> setters = new Dictionary<string, Action<T, object>>();

            // List of class property infos
            List<PropertyInfo> masterPropertyInfoFields = new List<PropertyInfo>();

            //List of T object data fields (DbColumnAttribute Values), and types.
            List<ObjectPropertyInfoField> masterObjectFields = new List<ObjectPropertyInfoField>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            masterPropertyInfoFields = typeof(T).GetProperties(flags)
               .Where(property => property.GetCustomAttribute<DbColumnAttribute>() != null)
               .Cast<PropertyInfo>()
               .ToList();

            foreach (var field in masterPropertyInfoFields)
            {
                var propertyInfo = typeof(T).GetProperty(field.Name);
                var columnName = field.GetCustomAttribute<DbColumnAttribute>().Name;
                setters.Add(columnName, Invoker.CreateSetter<T>(propertyInfo));
            }

            Parallel.ForEach(DataTable.AsEnumerable().ToList(),
                 (datarow) =>
                 {
                     T masterObj = new T();

                     foreach (var setter in setters)
                     {
                         if (!datarow.Table.Columns.Contains(setter.Key) || datarow[setter.Key] == null || datarow[setter.Key] == DBNull.Value) 
                         {
                             continue;
                         }
                         else
                         { 
                             setter.Value(masterObj, datarow[setter.Key]);
                         }
                     }

                     lock (dataList)
                     {
                         dataList.Add(masterObj);
                     }
                 }
            );

            return dataList;
        }


        /// <summary>
        /// Converts datatable to list<T> dynamically
        /// </summary>
        /// <typeparam name="T">Class name</typeparam>
        /// <param name="dataTable">data table to convert</param>
        /// <returns>List<T></returns>
        public static List<T> ConvertToList<T>(this DataTable DataTable, params Expression<Func<T, object>>[] path) where T : class, new()
        {
            var dataList = new List<T>();

            //
            // List of class property infos
            List<PropertyInfo> masterPropertyInfoFields = new List<PropertyInfo>();
            List<PropertyInfo> childPropertInfoFields = new List<PropertyInfo>();

            //
            // List of T object data fields (DbColumnAttribute Values), and types.
            Dictionary<string, Action<T, object>> SETTERS_MasterObject = new Dictionary<string, Action<T, object>>();
            Dictionary<string, Dictionary<string, Action<dynamic, object>>> SETTERS_ChildObjects = new Dictionary<string, Dictionary<string, Action<dynamic, object>>>();

            //
            // Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            Dictionary<string, string> expressionLookup = new Dictionary<string, string>();

            foreach (var t in path)
            {
                expressionLookup.Add((t.Body as MemberExpression).Member.Name, t.Body.Type.Name);
            }


            // Begin
            // Initialize Master the property info fields list
            masterPropertyInfoFields = typeof(T).GetProperties(flags)
                .Where(property => property.GetCustomAttribute<DbColumnAttribute>() != null)
                .Cast<PropertyInfo>()
                .ToList();

            // Initialize the master object setters dictionary
            foreach (var field in masterPropertyInfoFields)
            {
                var propertyInfo = typeof(T).GetProperty(field.Name);
                var columnName = field.GetCustomAttribute<DbColumnAttribute>().Name;
                SETTERS_MasterObject.Add(columnName, Invoker.CreateSetter<T>(propertyInfo));
            }

            // Read Datatable column names and types
            var dtlFieldNames = DataTable.Columns.Cast<DataColumn>()
                .Select(item => new
                {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

            //
            // Fill The data
            //foreach (var datarow in DataTable.AsEnumerable().ToList())
            //{
            Parallel.ForEach(DataTable.AsEnumerable().ToList(), (datarow) =>
            {
                // Create and instance of the master object type
                T masterObj = new T();

                //
                // Process the path, in case there are relations
                if (path.Count() > 0)
                {
                    // Initialize child the property info fields list
                    childPropertInfoFields = typeof(T).GetProperties(flags)
                        .Where(property => property.GetCustomAttribute<DataRelationAttribute>() != null && expressionLookup.Keys.Contains(property.Name))
                        .Cast<PropertyInfo>()
                        .ToList();


                    // Fill the Data for children objects
                    foreach (PropertyInfo property in childPropertInfoFields)
                    {
                        //
                        // Get the type of the child object
                        Type typeOfChildObject = property.PropertyType;

                        //
                        // Construct the dictionary of this child setters
                        // something like: Dictionary<FieldColumnName, SetterFunction>
                        // Dictionary Type: Dictionary<string, Action<T, object>>();
                        // First create the setter Action delegate Type: Action<T, object>
                        Type reflectionType = typeof(ReflectionHelper<>);
                        Type genericReflectionType = reflectionType.MakeGenericType(typeOfChildObject);
                        dynamic childReflection = Activator.CreateInstance(genericReflectionType);
                        dynamic childSetters = childReflection.GetReflectionDictionary();

                        //
                        // Get this child-object's relation name
                        var childRelationName = property.GetCustomAttribute<DataRelationAttribute>().Name;

                        //
                        // Get this child-object's fields
                        var childObjectFields = typeOfChildObject.GetProperties(flags)
                              .Where(item => item.GetCustomAttribute<DbColumnAttribute>() != null)
                              .Cast<PropertyInfo>()
                              .ToList();

                        //
                        // Foreach field in this child-object, add it's setter function to the childSetters dictionary
                        foreach (var field in childObjectFields)
                        {
                            var propertyInfo = typeOfChildObject.GetProperty(field.Name);
                            var columnName = field.GetCustomAttribute<DbColumnAttribute>().Name;

                            object[] setterMethodParams = new[] { propertyInfo };
                            MethodInfo buildUntypedSetterMethod = typeof(Invoker).GetMethod("CreateSetter");
                            MethodInfo genericSetterMethod = buildUntypedSetterMethod.MakeGenericMethod(typeOfChildObject);
                            dynamic setter = genericSetterMethod.Invoke(null, setterMethodParams);

                            // Fill the setter in he childSetters dictionary
                            // childSetters.Add(columnName, Invoker.BuildUntypedSetter<T>(propertyInfo));
                            // columnName = String.Format("{0}.{1}", childRelationName, columnName);
                            childSetters.Add(columnName, setter);
                        }

                        //
                        // Make an instance of the child-object type
                        dynamic childObj = Activator.CreateInstance(typeOfChildObject);

                        //
                        // Fill the child object through calling it's own setters
                        foreach (var setter in childSetters)
                        {
                            string columnName = String.Format("{0}.{1}", childRelationName, setter.Key);

                            try
                            {
                                object value = datarow[columnName];
                                setter.Value(childObj, value);
                            }
                            catch(Exception ex)
                            {
                                throw ex.InnerException;
                            }
                        }

                        //
                        // Set the values for the children-object in the master-object
                        foreach (PropertyInfo masterPropertyInfo in childPropertInfoFields)
                        {
                            if (masterPropertyInfo.PropertyType.Name == childObj.GetType().Name)
                            {
                                masterPropertyInfo.SetValue(masterObj, childObj);
                            }
                        }
                    }//end-outer-foreach
                }//end-if


                //
                // Fill master Object with its related properties values
                foreach (var setter in SETTERS_MasterObject)
                {
                    setter.Value(masterObj, datarow[setter.Key]);
                }

                lock (dataList)
                {
                    dataList.Add(masterObj);
                }
            });
            //}

            return dataList;
        }


        /// <summary>
        /// Converts List<T> to Datatable
        /// </summary>
        /// <typeparam name="T">ClassName</typeparam>
        /// <param name="list">List to be converted</param>
        /// <returns>Populated DataTable</returns>
        public static DataTable ConvertToDataTable<T>(this List<T> list) where T: class,new()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            Dictionary<string, Func<T, object>> getters = new Dictionary<string, Func<T, object>>();

            DataTable dt = new DataTable(typeof(T).Name);
            
            //Get all the properties
            List<PropertyInfo> masterPropertyInfoFields = typeof(T).GetProperties(flags)
                .Where(property => 
                    property.GetCustomAttribute<DbColumnAttribute>() != null &&
                    property.GetCustomAttribute<DbColumnAttribute>().Name != "PhoneCallsTableName")  //this is the only exception in the who solution 
                .Cast<PropertyInfo>()
               .ToList();

            int propertiesLength = masterPropertyInfoFields.ToArray().Length;

            foreach (var field in masterPropertyInfoFields)
            {
                var propertyInfo = typeof(T).GetProperty(field.Name);
                var columnName = field.GetCustomAttribute<DbColumnAttribute>().Name;

                var getter = Invoker.CreateGetter<T>(propertyInfo);

                getters.Add(columnName, getter);

                DataColumn col = new DataColumn(columnName);
                col.DataType = propertyInfo.PropertyType;

                if (col.DataType == typeof(decimal))
                {
                    col.DefaultValue = Convert.ToDecimal(0);
                }
                else if (col.DataType == typeof(string))
                {
                    col.DefaultValue = DBNull.Value;
                }
                else if (col.DataType == typeof(DateTime))
                {
                    col.DefaultValue = SqlDateTime.MinValue.Value;
                    col.DateTimeMode = DataSetDateTime.UnspecifiedLocal;
                }
                
                
                //Add Columns 
                dt.Columns.Add(col);
            }

            //only for Compisite Primary Key in our case it is phonecalls
            //dt.PrimaryKey = new[] { dt.Columns[0], dt.Columns[1] };

            //this object will be loacked during parallel loop
            object status = new object();

            //Add Rows
            Parallel.ForEach(list, (phonecall) => 
            {   
                lock (dt)
                {
                    if (phonecall == null)
                        return;
                    DataRow row = dt.NewRow();
                    foreach (var getter in getters)
                    {
                        //Validate DatetimeMIn and convert it to SQLDateTimeMin
                        if (dt.Columns[getter.Key].DataType == typeof(DateTime) && (DateTime)getter.Value(phonecall) == DateTime.MinValue)
                            row[getter.Key] = SqlDateTime.MinValue.Value;
                        else
                            row[getter.Key] = getter.Value(phonecall);
                    }
                    
                    dt.Rows.Add(row);
                }

            });

            return dt;

        }


        /// <summary>
        /// Gets the Name of DB table Field
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string Description(this Enum enumObject)
        {
            FieldInfo fieldInfo = enumObject.GetType().GetField(enumObject.ToString());

            DescriptionAttribute[] descAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descAttributes != null && descAttributes.Length > 0)
            {
                return descAttributes[0].Description;
            }
            else
            {
                return enumObject.ToString();
            }
        }


        /// <summary>
        /// Gets the DefaultValue attribute of the enum
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string Value(this Enum enumObject)
        {
            FieldInfo fieldInfo = enumObject.GetType().GetField(enumObject.ToString());

            DefaultValueAttribute[] valueAttributes = (DefaultValueAttribute[])fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false);

            if (valueAttributes != null && valueAttributes.Length > 0)
            {
                return valueAttributes[0].Value.ToString();
            }
            else
            {
                return enumObject.ToString();
            }
        }


        /// <summary>
        /// Return an enum object to a list of enums
        /// </summary>
        /// <typeparam name="T">Enum Object</typeparam>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T is not of System.Enum Type");
            }

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

    }

}
