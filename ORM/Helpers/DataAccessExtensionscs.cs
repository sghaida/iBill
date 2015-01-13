using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ORM.DataAccess;
using ORM.DataAttributes;

namespace ORM.Helpers
{
    public static class DataAccessExtensionscs
    {
        private static DBLib DBRoutines = new DBLib();

        public static T GetWithRelations<T>(this T source, params Expression<Func<T, object>>[] path) where T : DataModel, new()
        {
            DataSourceSchema<T> Schema = new DataSourceSchema<T>();

            // Table Relations Map
            // To be sent to the DB Lib for SQL Query generation
            List<SqlJoinRelation> TableRelationsMap = new List<SqlJoinRelation>();
            List<DbRelation> DbRelationsList = new List<DbRelation>();

            //
            // Database related
            // Where conditions dictionary
            DataTable dt = new DataTable();
            string finalDataSourceName = string.Empty;
            List<string> thisModelTableColumns = new List<string>();
            Dictionary<string, object> whereConditions = new Dictionary<string, object>();


            // This will hold the information about the sub joins object types          
            Dictionary<string, string> expressionLookup = new Dictionary<string, string>();

            foreach (var t in path)
            {
                expressionLookup.Add((t.Body as MemberExpression).Member.Name, t.Body.Type.Name);
            }


            //
            // Get the Relations Fields from the Schema 
            DbRelationsList = Schema.DataFields
                .Where(field =>
                    field.Relation != null &&
                    expressionLookup.Values.Contains(field.Relation.WithDataModel.Name) &&
                    expressionLookup.Keys.Contains(field.Name))
                .Select<DataField, DbRelation>(field => field.Relation).
                ToList<DbRelation>();


            //
            // Start processing the list of table relations
            if (DbRelationsList != null && DbRelationsList.Count() > 0)
            {
                //Foreach relation in the relations list, process it and construct the big TablesRelationsMap
                foreach (var relation in DbRelationsList)
                {
                    //Create a temporary map for this target table relation
                    var joinedTableInfo = new SqlJoinRelation();

                    //Get the data model we're in relation with.
                    Type relationType = relation.WithDataModel;

                    //Build a data source schema for the data model we're in relation with.
                    var generalModelSchemaType = typeof(DataSourceSchema<>);
                    var specialModelSchemaType = generalModelSchemaType.MakeGenericType(relationType);
                    dynamic joinedModelSchema = Activator.CreateInstance(specialModelSchemaType);

                    //Get it's Data Fields.
                    List<DataField> joinedModelFields = joinedModelSchema.GetDataFields();

                    //Get the table column names - exclude the ID field name.
                    List<string> joinedModelTableColumns = joinedModelFields
                        .Where(field => field.TableField != null)
                        .Select<DataField, string>(field => field.TableField.ColumnName)
                        .ToList<string>();

                    //Get the field that describes the relation key from the target model schema
                    DataField joinedModelKey = joinedModelFields.Find(item => item.TableField != null && item.Name == relation.OnDataModelKey);

                    //Get the field that describes our key on which we are in relation with the target model
                    DataField thisKey = Schema.DataFields.Find(item => item.TableField != null && item.Name == relation.ThisKey);

                    if (thisKey != null && joinedModelKey != null)
                    {
                        //Initialize the temporary map and add it to the original relations map
                        joinedTableInfo.RelationName = relation.RelationName;
                        joinedTableInfo.RelationType = relation.RelationType;
                        joinedTableInfo.MasterTableName = Schema.DataSourceName;
                        joinedTableInfo.MasterTableKey = thisKey.TableField.ColumnName;
                        joinedTableInfo.JoinedTableName = joinedModelSchema.GetDataSourceName();
                        joinedTableInfo.JoinedTableKey = joinedModelKey.TableField.ColumnName;
                        joinedTableInfo.JoinedTableColumns = joinedModelTableColumns;

                        //Add the relation keys to the TableRelationsMap
                        TableRelationsMap.Add(joinedTableInfo);
                    }

                }//end-foreach

            }//end-outer-if


            //
            // Get the ID Field to find the relations for.
            // If the ID Field was not found, return an empty instance of the object.
            var IDField = Schema.DataFields.Find(field => field.TableField != null && field.TableField.IsIDField == true);

            if (IDField != null)
            {
                var dataObjectAttr = source.GetType().GetProperty(IDField.Name);

                var dataObjectAttrValue = dataObjectAttr.GetValue(source, null);

                // Put the ID Field in the WHERE CONDITIONS
                if (dataObjectAttrValue != null)
                {
                    whereConditions.Add(IDField.TableField.ColumnName, Convert.ChangeType(dataObjectAttrValue, IDField.TableField.FieldType));
                }
                else
                {
                    return source;
                }
            }
            else
            {
                return source;
            }

            //
            // Get our table columns from the schema
            thisModelTableColumns = Schema.DataFields
                .Where(field => field.TableField != null)
                .Select<DataField, string>(
                field => field.TableField.ColumnName)
                .ToList<string>();

            //
            // Query the data-srouce
            dt = DBRoutines.SELECT_WITH_JOIN(Schema.DataSourceName, thisModelTableColumns, whereConditions, TableRelationsMap, 1);

            // Return data
            var data = dt.ConvertToList<T>(path);

            if (data != null && data.Count > 0)
            {
                return data.First();
            }
            else
            {
                return source;
            }
        }
        
        /// <summary>
        /// This extension Method works for IEnumerable Types, it exactly select from the Database with first level Join
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">IEnumerable data which to be evaluated and filled by the extension method</param>
        /// <param name="DataSourceName">The DataSource that you wish to select from incase of Distributed Datasources</param>
        /// <param name="path">the path of the relation such as item=>item.x</param>
        /// <returns>IEnumerable with populated relation </returns>
        public static IEnumerable<T> GetWithRelations<T>(this IEnumerable<T> source, string DataSourceName,params Expression<Func<T, object>>[] path) where T : DataModel, new()
        {
            DataSourceSchema<T> Schema = new DataSourceSchema<T>();

            //Table Relations Map
            //To be sent to the DB Lib for SQL Query generation
            List<SqlJoinRelation> TableRelationsMap = new List<SqlJoinRelation>();

            List<DbRelation> DbRelationsList = new List<DbRelation>();

            //This will hold the information about the sub joins object types          
            Dictionary<string, string> expressionLookup = new Dictionary<string, string>();

            foreach (var t in path)
            {
                expressionLookup.Add((t.Body as MemberExpression).Member.Name, t.Body.Type.Name);
            }

            DbRelationsList = Schema.DataFields
                .Where(field => 
                    field.Relation != null &&
                    expressionLookup.Values.Contains(field.Relation.WithDataModel.Name) &&
                    expressionLookup.Keys.Contains(field.Name))
                .Select<DataField, DbRelation>(field => field.Relation).
                ToList<DbRelation>();


            //Start processing the list of table relations
            if (DbRelationsList != null && DbRelationsList.Count() > 0)
            {
                //Foreach relation in the relations list, process it and construct the big TablesRelationsMap
                foreach (var relation in DbRelationsList)
                {
                    //Create a temporary map for this target table relation
                    var joinedTableInfo = new SqlJoinRelation();

                    //Get the data model we're in relation with.
                    Type relationType = relation.WithDataModel;

                    //Build a data source schema for the data model we're in relation with.
                    var generalModelSchemaType = typeof(DataSourceSchema<>);
                    var specialModelSchemaType = generalModelSchemaType.MakeGenericType(relationType);
                    dynamic joinedModelSchema = Activator.CreateInstance(specialModelSchemaType);

                    //Get it's Data Fields.
                    List<DataField> joinedModelFields = joinedModelSchema.GetDataFields();

                    //Get the table column names - exclude the ID field name.
                    List<string> joinedModelTableColumns = joinedModelFields
                        .Where(field => field.TableField != null)
                        .Select<DataField, string>(field => field.TableField.ColumnName)
                        .ToList<string>();

                    //Get the field that describes the relation key from the target model schema
                    DataField joinedModelKey = joinedModelFields.Find(item => item.TableField != null && item.Name == relation.OnDataModelKey);

                    //Get the field that describes our key on which we are in relation with the target model
                    DataField thisKey = Schema.DataFields.Find(item => item.TableField != null && item.Name == relation.ThisKey);

                    if (thisKey != null && joinedModelKey != null)
                    {
                        //Initialize the temporary map and add it to the original relations map
                        joinedTableInfo.RelationName = relation.RelationName;
                        joinedTableInfo.RelationType = relation.RelationType;
                        if (DataSourceName == null)
                        {
                            joinedTableInfo.MasterTableName = Schema.DataSourceName;
                        }
                        else 
                        {
                            joinedTableInfo.MasterTableName = DataSourceName;
                        }
                        joinedTableInfo.MasterTableKey = thisKey.TableField.ColumnName;
                        joinedTableInfo.JoinedTableName = joinedModelSchema.GetDataSourceName();
                        joinedTableInfo.JoinedTableKey = joinedModelKey.TableField.ColumnName;
                        joinedTableInfo.JoinedTableColumns = joinedModelTableColumns;

                        //Add the relation keys to the TableRelationsMap
                        TableRelationsMap.Add(joinedTableInfo);
                    }

                }//end-foreach

            }//end-outer-if

            DataTable dt = new DataTable();
            string finalDataSourceName = string.Empty;

            if (DataSourceName == null)
            {
                finalDataSourceName = Schema.DataSourceName;
            }
            else 
            {
                finalDataSourceName = DataSourceName;
            }
            

            List<string> thisModelTableColumns;

            //Get our table columns from the schema
            thisModelTableColumns = Schema.DataFields
                .Where(field => field.TableField != null)
                .Select<DataField, string>(
                field => field.TableField.ColumnName)
                .ToList<string>();

            dt = DBRoutines.SELECT_WITH_JOIN(finalDataSourceName, thisModelTableColumns, null, TableRelationsMap, 0);

            return dt.ConvertToList<T>(path);
        }

        /// <summary>
        /// This extension Method works for IEnumerable Types, it exactly select from the Database with first level Join
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">IEnumerable data which to be evaluated and filled by the extension method</param>
        /// <param name="path">the path of the relation such as item=>item.x</param>
        /// <returns>IEnumerable with populated relation </returns>
        public static IEnumerable<T> GetWithRelations<T>(this IEnumerable<T> source, params Expression<Func<T, object>>[] path) where T : DataModel, new()
        {
            DataSourceSchema<T> Schema = new DataSourceSchema<T>();

            //Table Relations Map
            //To be sent to the DB Lib for SQL Query generation
            List<SqlJoinRelation> TableRelationsMap = new List<SqlJoinRelation>();

            List<DbRelation> DbRelationsList = new List<DbRelation>();

            //This will hold the information about the sub joins object types          
            Dictionary<string, string> expressionLookup = new Dictionary<string, string>();

            foreach (var t in path)
            {
                expressionLookup.Add((t.Body as MemberExpression).Member.Name, t.Body.Type.Name);
            }

            DbRelationsList = Schema.DataFields
                .Where(field =>
                    field.Relation != null &&
                    expressionLookup.Values.Contains(field.Relation.WithDataModel.Name) &&
                    expressionLookup.Keys.Contains(field.Name))
                .Select<DataField, DbRelation>(field => field.Relation).
                ToList<DbRelation>();


            //Start processing the list of table relations
            if (DbRelationsList != null && DbRelationsList.Count() > 0)
            {
                //Foreach relation in the relations list, process it and construct the big TablesRelationsMap
                foreach (var relation in DbRelationsList)
                {
                    //Create a temporary map for this target table relation
                    var joinedTableInfo = new SqlJoinRelation();

                    //Get the data model we're in relation with.
                    Type relationType = relation.WithDataModel;

                    //Build a data source schema for the data model we're in relation with.
                    var generalModelSchemaType = typeof(DataSourceSchema<>);
                    var specialModelSchemaType = generalModelSchemaType.MakeGenericType(relationType);
                    dynamic joinedModelSchema = Activator.CreateInstance(specialModelSchemaType);

                    //Get it's Data Fields.
                    List<DataField> joinedModelFields = joinedModelSchema.GetDataFields();

                    //Get the table column names - exclude the ID field name.
                    List<string> joinedModelTableColumns = joinedModelFields
                        .Where(field => field.TableField != null)
                        .Select<DataField, string>(field => field.TableField.ColumnName)
                        .ToList<string>();

                    //Get the field that describes the relation key from the target model schema
                    DataField joinedModelKey = joinedModelFields.Find(item => item.TableField != null && item.Name == relation.OnDataModelKey);

                    //Get the field that describes our key on which we are in relation with the target model
                    DataField thisKey = Schema.DataFields.Find(item => item.TableField != null && item.Name == relation.ThisKey);

                    if (thisKey != null && joinedModelKey != null)
                    {
                        //Initialize the temporary map and add it to the original relations map
                        joinedTableInfo.RelationName = relation.RelationName;
                        joinedTableInfo.RelationType = relation.RelationType;
                        joinedTableInfo.MasterTableName = Schema.DataSourceName;
                        joinedTableInfo.MasterTableKey = thisKey.TableField.ColumnName;
                        joinedTableInfo.JoinedTableName = joinedModelSchema.GetDataSourceName();
                        joinedTableInfo.JoinedTableKey = joinedModelKey.TableField.ColumnName;
                        joinedTableInfo.JoinedTableColumns = joinedModelTableColumns;

                        //Add the relation keys to the TableRelationsMap
                        TableRelationsMap.Add(joinedTableInfo);
                    }

                }//end-foreach

            }//end-outer-if

            DataTable dt = new DataTable();
            
            List<string> thisModelTableColumns;

            //Get our table columns from the schema
            thisModelTableColumns = Schema.DataFields
                .Where(field => field.TableField != null)
                .Select<DataField, string>(
                field => field.TableField.ColumnName)
                .ToList<string>();

            dt = DBRoutines.SELECT_WITH_JOIN(Schema.DataSourceName, thisModelTableColumns, null, TableRelationsMap, 0);

            return dt.ConvertToList<T>(path);
        }


        [Obsolete("This Function is deprecated. kindly use GetWithRelations instead")]
        public static IEnumerable<T> IncludeRelation<T>(this IEnumerable<T> source,string DataSourceName, params Expression<Func<T, object>>[] path) where T : DataModel, new()
        {
            DataSourceSchema<T> Schema = new DataSourceSchema<T>();
            //Table Relations Map
            //To be sent to the DB Lib for SQL Query generation
            List<SqlJoinRelation> TableRelationsMap = new List<SqlJoinRelation>();

            List<DbRelation> DbRelationsList = new List<DbRelation>();

            //This will hold the information about the sub joins object types          
            Dictionary<string, string> expressionLookup = new Dictionary<string, string>();
            foreach (var t in path)
            {
                expressionLookup.Add((t.Body as MemberExpression).Member.Name, t.Body.Type.Name);
            }

            DbRelationsList = Schema.DataFields.Where(field => field.Relation != null &&
                expressionLookup.Values.Contains(field.Relation.WithDataModel.Name) &&
                expressionLookup.Keys.Contains(field.Name)
                ).
                Select<DataField, DbRelation>(field => field.Relation).
                ToList<DbRelation>();


            //Start processing the list of table relations
            if (DbRelationsList != null && DbRelationsList.Count() > 0)
            {
                //Foreach relation in the relations list, process it and construct the big TablesRelationsMap
                foreach (var relation in DbRelationsList)
                {
                    //Create a temporary map for this target table relation
                    var joinedTableInfo = new SqlJoinRelation();

                    //Get the data model we're in relation with.
                    Type relationType = relation.WithDataModel;

                    //Build a data source schema for the data model we're in relation with.
                    var generalModelSchemaType = typeof(DataSourceSchema<>);
                    var specialModelSchemaType = generalModelSchemaType.MakeGenericType(relationType);
                    dynamic joinedModelSchema = Activator.CreateInstance(specialModelSchemaType);

                    //Get it's Data Fields.
                    List<DataField> joinedModelFields = joinedModelSchema.GetDataFields();

                    //Get the table column names - exclude the ID field name.
                    List<string> joinedModelTableColumns = joinedModelFields
                        .Where(field => field.TableField != null)
                        .Select<DataField, string>(field => field.TableField.ColumnName)
                        .ToList<string>();

                    //Get the field that describes the relation key from the target model schema
                    DataField joinedModelKey = joinedModelFields.Find(item => item.TableField != null && item.Name == relation.OnDataModelKey);

                    //Get the field that describes our key on which we are in relation with the target model
                    DataField thisKey = Schema.DataFields.Find(item => item.TableField != null && item.Name == relation.ThisKey);

                    if (thisKey != null && joinedModelKey != null)
                    {
                        //Initialize the temporary map and add it to the original relations map
                        joinedTableInfo.RelationName = relation.RelationName;
                        joinedTableInfo.RelationType = relation.RelationType;
                        
                        
                        joinedTableInfo.MasterTableName = DataSourceName;
                        joinedTableInfo.MasterTableKey = thisKey.TableField.ColumnName;
                        joinedTableInfo.JoinedTableName = joinedModelSchema.GetDataSourceName();
                        joinedTableInfo.JoinedTableKey = joinedModelKey.TableField.ColumnName;
                        joinedTableInfo.JoinedTableColumns = joinedModelTableColumns;

                        //Add the relation keys to the TableRelationsMap
                        TableRelationsMap.Add(joinedTableInfo);
                    }

                }//end-foreach

            }//end-outer-if

            DataTable dt = new DataTable();
            
            List<string> thisModelTableColumns;

            //Get our table columns from the schema
            thisModelTableColumns = Schema.DataFields
                .Where(field => field.TableField != null)
                .Select<DataField, string>(
                field => field.TableField.ColumnName)
                .ToList<string>();

            dt = DBRoutines.SELECT_WITH_JOIN(DataSourceName, thisModelTableColumns, null, TableRelationsMap, 0);

            return dt.ConvertToList<T>(path);
        }
    }

}
