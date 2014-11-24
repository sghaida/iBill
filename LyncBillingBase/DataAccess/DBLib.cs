using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.Libs;


namespace LyncBillingBase.DataAccess
{
    public class DBLib
    {
        //Load DLL Configs
        public  static LoadConfigs config = new LoadConfigs();

        //Get the connection String
        public static string ConnectionString_Lync = config.DllConfig.ConnectionStrings.ConnectionStrings["LyncConnectionString"].ConnectionString;

        private OleDbConnection DBInitializeConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }

        //SELECT with SQL query
        public DataTable SELECTFROMSQL(string sqlQuery, string customConnectionString = null)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            OleDbConnection conn;

            //Initialize connections
            if (!string.IsNullOrEmpty(customConnectionString))
                conn = DBInitializeConnection(customConnectionString);
            else
                conn = DBInitializeConnection(ConnectionString_Lync);

            //Execute SQL Query
            OleDbCommand comm = new OleDbCommand(sqlQuery, conn);


            try
            {
                conn.Open();
                dr = comm.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public DataTable SELECT(string tableName, string wherePart, string customConnectionString = null) 
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            OleDbConnection conn;

            //Initialize connections
            if (!string.IsNullOrEmpty(customConnectionString))
                conn = DBInitializeConnection(customConnectionString);
            else
                conn = DBInitializeConnection(ConnectionString_Lync);

            string sqlQuery = string.Format("SELECT * FROM {0} WHERE {1}",tableName,wherePart);

            //Execute SQL Query
            OleDbCommand comm = new OleDbCommand(sqlQuery, conn);


            try
            {
                conn.Open();
                dr = comm.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        /// <summary>
        /// Construct Generic Select Statemnet 
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="whereField">Where statemnet Field</param>
        /// <param name="whereValue">Where statemnet Value</param>
        /// <returns> DataTable Object</returns>
        public DataTable SELECT(string tableName, string whereField, object whereValue)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder selectedfields = new StringBuilder();

            if (whereValue.GetType().Equals(typeof(int)) ||
                whereValue.GetType().Equals(typeof(double)) ||
                whereValue.GetType().Equals(typeof(decimal)) ||
                whereValue.GetType().Equals(typeof(Int32)) ||
                whereValue.GetType().Equals(typeof(Int64)))
                selectQuery = string.Format("SELECT * FROM  [{0}] WHERE [{1}]={2}", tableName, whereField, whereValue);
            else
                selectQuery = string.Format("SELECT * FROM  [{0}] WHERE [{1}]='{2}'", tableName, whereField, whereValue);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(selectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

            return dt;
        }

        public DataTable SELECT(string tableName)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder selectedfields = new StringBuilder();

            selectQuery = string.Format("SELECT * FROM  [{0}]", tableName);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(selectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

            return dt;
        }

        /// <summary>
        /// Construct Generic Select Statemnet 
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="fields">List of Fields to be fetched from Database</param>
        /// <param name="whereClause"> A dictionary which holds the fields and its related values to be able to construct Where Statemnet
        /// 1. Null if there is no condition
        /// 2. Dictionary holds fields names and values if there is a condition
        /// </param>
        /// <param name="limits">Holds how many rows to be fetched from the database table. 
        /// 1. 0 if all Rows 
        /// 2. Value for number of rows</param>
        /// <returns>DataTable Object</returns>
        public DataTable SELECT(string tableName, List<string> fields, Dictionary<string, object> whereClause, int limits)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder selectedfields = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();
            StringBuilder orderBy = new StringBuilder();

            if (tableName == "PhoneCalls")
                orderBy.Append("ORDER BY [SessionIdTime] DESC");
            else
                orderBy.Append("");

            if (fields != null)
            {
                if (fields.Count != 0)
                {
                    foreach (string fieldName in fields)
                    {
                        selectedfields.Append(fieldName + ",");
                    }
                    selectedfields.Remove(selectedfields.Length - 1, 1);
                }
                else
                    selectedfields.Append("*");
            }
            else
                selectedfields.Append("*");

            if (whereClause != null && whereClause.Count != 0)
            {
                whereStatement.Append("WHERE ");
                foreach (KeyValuePair<string, object> pair in whereClause)
                {
                    if (pair.Value == null)
                    {
                        whereStatement.Append("[" + pair.Key + "] IS NULL" + " AND ");
                    }
                    else if (pair.Value.ToString() == "!null")
                    {
                        whereStatement.Append("[" + pair.Key + "] IS NOT NULL" + " AND ");
                    }
                    else
                    {
                        Type valueType = pair.Value.GetType();
                        if (valueType == typeof(int) || valueType == typeof(Double))
                        {
                            whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                        }
                        else
                        {
                            whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' COLLATE SQL_Latin1_General_CP1_CI_AS AND ");
                        }
                    }
                }
                whereStatement.Remove(whereStatement.Length - 5, 5);
            }

            if (limits == 0)
            {
                if (whereClause != null)
                    selectQuery = string.Format("SELECT {0} FROM  [{1}] {2} {3}", selectedfields.ToString(), tableName, whereStatement.ToString(), orderBy);
                else
                    selectQuery = string.Format("SELECT {0} FROM  [{1}] {2}", selectedfields.ToString(), tableName, orderBy);
            }
            else
            {
                if (whereClause != null)
                    selectQuery = string.Format("SELECT TOP({0}) {1} FROM [{2}] {3} {4}", limits, selectedfields.ToString(), tableName, whereStatement.ToString(), orderBy);
                else
                    selectQuery = string.Format("SELECT TOP({0}) {1} FROM [{2}] {3}", limits, selectedfields.ToString(), tableName, orderBy);
            }

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(selectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();

                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

            return dt;
        }

        /// <summary>
        /// Construct Generic Select Statemnet 
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="fieldsList">List of Fields to be fetched from Database</param>
        /// <param name="whereClause"> A dictionary which holds the fields and its related values to be able to construct Where Statemnet
        /// 1. Null if there is no condition
        /// 2. Dictionary holds fields names and values if there is a condition
        /// </param>
        /// <param name="limits">Holds how many rows to be fetched from the database table. 
        /// 1. 0 if all Rows 
        /// 2. Value for number of rows</param>
        /// <param name="setWhereStatementOperatorToOR">The default operator in the where statement is "AND", if you set this one to true, the operator will be turned to "OR" in the where statement</param>
        /// <returns>DataTable Object</returns>
        public DataTable SELECT(string tableName, List<string> fieldsList, Dictionary<string, object> whereClause, int limits, bool setWhereStatementOperatorToOR = false)
        {
            DataTable dt = new DataTable();
            OleDbDataReader dr;
            string selectQuery = string.Empty;
            string OPERATOR = (setWhereStatementOperatorToOR == true) ? " OR " : " AND ";

            StringBuilder selectedfields = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();
            StringBuilder orderBy = new StringBuilder();


            //Handle tableName
            if (tableName.Contains("Phonecalls"))
            {
                orderBy.Append("ORDER BY [SessionIdTime] DESC");
            }
            else
            {
                orderBy.Append("");
            }


            //Handle the fields collection
            if (fieldsList != null)
            {
                if (fieldsList.Count != 0)
                {
                    foreach (string field in fieldsList)
                    {
                        //selectedfields.Append(fieldName + ",");
                        if (!string.IsNullOrEmpty(field))
                        {
                            if (field.Contains("COUNT") || field.Contains("SUM") || field.Contains("YEAR") || field.Contains("MONTH") || field.Contains("DISTINCT"))
                                selectedfields.Append(field + ",");
                            else
                                selectedfields.Append("[" + field + "],");
                        }
                    }
                    selectedfields.Remove(selectedfields.Length - 1, 1);
                }
                else
                    selectedfields.Append("*");
            }
            else
            {
                selectedfields.Append("*");
            }


            //Handle the whereClause collection
            if (whereClause != null && whereClause.Count != 0)
            {
                whereStatement.Append("WHERE ");

                foreach (KeyValuePair<string, object> pair in whereClause)
                {
                    if (pair.Value == null)
                    {
                        whereStatement.Append("[" + pair.Key + "] IS NULL " + OPERATOR);
                    }

                    else if (pair.Value.ToString() == "!null")
                    {
                        whereStatement.Append("[" + pair.Key + "] IS NOT NULL " + OPERATOR);
                    }

                    else if (pair.Value.ToString() == "!=0")
                    {
                        whereStatement.Append("[" + pair.Key + "] <> 0 " + OPERATOR);
                    }

                    else if (pair.Value is string && pair.Value.ToString().ToLower().Contains("like"))
                    {
                        whereStatement.Append("[" + pair.Key + "] " + pair.Value + OPERATOR);
                    }

                    else if (pair.Value is string && (pair.Value.ToString()).Contains("BETWEEN"))
                    {
                        whereStatement.Append("[" + pair.Key + "]  " + pair.Value);

                        whereStatement.Append(" AND ");
                    }

                    else if (pair.Value is List<int>)
                    {
                        whereStatement.Append("[" + pair.Key + "] in ( ");

                        foreach (var item in (List<int>)pair.Value)
                        {
                            whereStatement.Append(item.ToString() + ",");
                        }
                        //Remove last ','
                        whereStatement.Remove(whereStatement.Length - 1, 1);

                        whereStatement.Append(" ) " + OPERATOR);
                    }

                    else if (pair.Value is List<string>)
                    {
                        whereStatement.Append("[" + pair.Key + "] in ( ");

                        foreach (var item in (List<string>)pair.Value)
                        {
                            whereStatement.Append(item.ToString() + ",");
                        }
                        //Remove last ','
                        whereStatement.Remove(whereStatement.Length - 1, 1);

                        whereStatement.Append(" ) " + OPERATOR);
                    }

                    else
                    {
                        Type valueType = pair.Value.GetType();
                        if (valueType == typeof(int) || valueType == typeof(Double))
                        {
                            whereStatement.Append("[" + pair.Key + "]=" + pair.Value + OPERATOR);
                        }
                        else
                        {
                            whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "'" + OPERATOR);
                        }
                    }
                }

                //Trim the whereStatement
                if (setWhereStatementOperatorToOR == true)
                    whereStatement.Remove(whereStatement.Length - 4, 4);
                else
                    whereStatement.Remove(whereStatement.Length - 5, 5);
            }

            if (limits == 0)
            {
                if (whereClause != null)
                    selectQuery = string.Format("SELECT {0} FROM  [{1}] {2} {3}", selectedfields.ToString(), tableName, whereStatement.ToString(), orderBy);
                else
                    selectQuery = string.Format("SELECT {0} FROM  [{1}] {2}", selectedfields.ToString(), tableName, orderBy);
            }
            else
            {
                if (whereClause != null)
                    selectQuery = string.Format("SELECT TOP({0}) {1} FROM [{2}] {3} {4}", limits, selectedfields.ToString(), tableName, whereStatement.ToString(), orderBy);
                else
                    selectQuery = string.Format("SELECT TOP({0}) {1} FROM [{2}] {3}", limits, selectedfields.ToString(), tableName, orderBy);
            }

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(selectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();

                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

            return dt;
        }

        public DataTable SELECT_FROM_FUNCTION(string tableName, List<object> functionParams, Dictionary<string, object> whereClause)
        {
            DataTable dt = new DataTable();

            OleDbDataReader dr;
            string selectQuery = string.Empty;

            StringBuilder Parameters = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();

            if (functionParams != null && functionParams.Count != 0)
            {

                foreach (object obj in functionParams)
                {
                    Type valueType = obj.GetType();

                    if (valueType == typeof(string) || valueType == typeof(DateTime))
                        Parameters.Append("'" + obj.ToString() + "',");
                    else
                        Parameters.Append(obj + ",");
                }
                Parameters.Remove(Parameters.Length - 1, 1);
            }

            if (whereClause != null && whereClause.Count != 0)
            {
                whereStatement.Append("WHERE ");
                foreach (KeyValuePair<string, object> pair in whereClause)
                {
                    Type valueType = pair.Value.GetType();

                    if (valueType == typeof(int) || valueType == typeof(Double))
                        whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                    else
                        whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");
                }
                whereStatement.Remove(whereStatement.Length - 5, 5);
            }

            if (whereClause != null && whereClause.Count != 0)
                selectQuery = string.Format("SELECT * FROM [{0}] ({1}) WHERE {2}", tableName, Parameters, whereStatement);
            else
                selectQuery = string.Format("SELECT * FROM [{0}] ({1})", tableName, Parameters);


            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(selectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();

                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

            return dt;
        }

        public DataTable SELECT_FROM_FUNCTION(string databaseFunctionName, List<object> functionParams, Dictionary<string, object> whereClause, List<string> selectColumnsList = null, List<string> groupByColumnsList = null)
        {
            DataTable dt = new DataTable();

            OleDbDataReader dr;
            string FinalSelectQuery = string.Empty;

            StringBuilder Parameters = new StringBuilder();
            StringBuilder WhereStatement = new StringBuilder();
            StringBuilder SelectColumns = new StringBuilder();
            StringBuilder GroupByFields = new StringBuilder();

            if (functionParams != null && functionParams.Count != 0)
            {
                foreach (object obj in functionParams)
                {
                    Type valueType = obj.GetType();

                    if (valueType == typeof(string) || valueType == typeof(DateTime))
                        Parameters.Append("'" + obj.ToString() + "',");
                    else
                        Parameters.Append(obj + ",");
                }
                Parameters.Remove(Parameters.Length - 1, 1);
            }

            if (selectColumnsList != null && selectColumnsList.Count != 0)
            {
                foreach (string field in selectColumnsList)
                {
                    if (!string.IsNullOrEmpty(field))
                    {
                        if (field.Contains("COUNT") || field.Contains("SUM") || field.Contains("YEAR") || field.Contains("MONTH") || field.Contains("DISTINCT"))
                            SelectColumns.Append(field + ",");
                        else
                            SelectColumns.Append("[" + field + "],");
                    }
                }
                SelectColumns.Remove(SelectColumns.Length - 1, 1);
            }

            if (groupByColumnsList != null && groupByColumnsList.Count != 0)
            {
                foreach (string field in groupByColumnsList)
                {
                    if (!string.IsNullOrEmpty(field))
                    {
                        GroupByFields.Append("[" + field + "],");
                    }
                }
                GroupByFields.Remove(GroupByFields.Length - 1, 1);
            }

            if (whereClause != null && whereClause.Count != 0)
            {
                foreach (KeyValuePair<string, object> pair in whereClause)
                {
                    if (pair.Value == null)
                    {
                        WhereStatement.Append("[" + pair.Key + "] IS NULL" + " AND ");
                    }

                    else if (pair.Value.ToString() == "!null")
                    {
                        WhereStatement.Append("[" + pair.Key + "] IS NOT NULL" + " AND ");
                    }

                    else if (pair.Value.ToString() == "!=0")
                    {
                        WhereStatement.Append("[" + pair.Key + "] <> 0" + " AND ");
                    }

                    else if (pair.Value is string && (pair.Value.ToString()).Contains("BETWEEN"))
                    {
                        WhereStatement.Append("[" + pair.Key + "]  " + pair.Value);

                        WhereStatement.Append(" AND ");
                    }

                    else if (pair.Value is List<int>)
                    {
                        WhereStatement.Append("[" + pair.Key + "] in ( ");

                        foreach (var item in (List<int>)pair.Value)
                        {
                            WhereStatement.Append(item.ToString() + ",");
                        }
                        //Remove last ','
                        WhereStatement.Remove(WhereStatement.Length - 1, 1);

                        WhereStatement.Append(" ) AND ");
                    }

                    else if (pair.Value is List<string>)
                    {
                        WhereStatement.Append("[" + pair.Key + "] in ( ");

                        foreach (var item in (List<string>)pair.Value)
                        {
                            WhereStatement.Append("'" + item.ToString() + "',");
                        }
                        //Remove last ','
                        WhereStatement.Remove(WhereStatement.Length - 1, 1);

                        WhereStatement.Append(" ) AND ");
                    }

                    else
                    {
                        Type valueType = pair.Value.GetType();
                        if (valueType == typeof(int) || valueType == typeof(Double))
                        {
                            WhereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                        }
                        else
                        {
                            WhereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");
                        }
                    }
                }
                WhereStatement.Remove(WhereStatement.Length - 5, 5);
            }

            if (selectColumnsList != null && selectColumnsList.Count > 0)
                FinalSelectQuery = String.Format("SELECT {0} ", SelectColumns.ToString());
            else
                FinalSelectQuery = String.Format("SELECT * ");

            if (whereClause != null && whereClause.Count > 0)
                FinalSelectQuery = string.Format("{0} FROM [{1}] ({2}) WHERE {3}", FinalSelectQuery, databaseFunctionName, Parameters.ToString(), WhereStatement.ToString());
            else
                FinalSelectQuery = string.Format("{0} FROM [{1}] ({2})", FinalSelectQuery, databaseFunctionName, Parameters.ToString());

            if (groupByColumnsList != null && groupByColumnsList.Count > 0)
                FinalSelectQuery = string.Format("{0} GROUP BY {1} ", FinalSelectQuery, GroupByFields.ToString());
            else
                FinalSelectQuery = string.Format("{0}", FinalSelectQuery);


            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(FinalSelectQuery, conn);

            try
            {
                conn.Open();
                dr = comm.ExecuteReader();

                dt.Load(dr);
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

            return dt;
        }

        /// <summary>
        /// Execute a store procedure with some parameters
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <returns>Row ID </returns>
        public void EXECUTE_STORE_PROCEDURE(string storeProcedureName, Dictionary<string, object> parameters)
        {
            OleDbConnection conn;
            OleDbCommand comm;
            OleDbTransaction transaction;
            int numberOfRowsAffected = 0;
            string FinalSelectQuery = string.Empty;
            StringBuilder Parameters = new StringBuilder();


            if (parameters != null && parameters.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    string keyValueString = string.Empty;

                    keyValueString = String.Format("@{0} = N'{1}', ", pair.Key, pair.Value);
                    Parameters.Append(keyValueString);
                }

                Parameters.Remove(Parameters.Length - 2, 2);
            }


            //Construct the EXEC FINAL QUERY
            //SET TRANSACTION ISOLATION LEVEL SNAPSHOT; EXEC ....
            if (Parameters.Length > 0)
                FinalSelectQuery = String.Format("SET TRANSACTION ISOLATION LEVEL SNAPSHOT; EXEC [dbo].[{0}] {1}", storeProcedureName, Parameters.ToString());
            else
                FinalSelectQuery = String.Format("SET TRANSACTION ISOLATION LEVEL SNAPSHOT; EXEC [dbo].[{0}]", storeProcedureName);


            //Establish DB connection and create a command.
            //Set the connection timeout to 20 minutes in case there is no timeout already defined.
            var localConnectionString = ConnectionString_Lync;
            if (!localConnectionString.Contains("ConnectionTimout"))
            {
                localConnectionString += ";ConnectionTimeout=1800";
            }

            conn = DBInitializeConnection(localConnectionString);
            comm = new OleDbCommand(FinalSelectQuery, conn);

            try
            {
                //conn.Open();
                //transaction = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                //comm.Transaction = transaction;

                conn.Open();
                comm.CommandTimeout = (30 * 60); //30 minutes
                numberOfRowsAffected = comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            //return numberOfRowsAffected;
        }

        /// <summary>
        /// Insert by EXECUTING NATIVE SQL QUERY
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public int INSERT(string SQL) 
        {
            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(SQL, conn);

            int recordID = 0;
            try
            {
                conn.Open();
                recordID = Convert.ToInt32(comm.ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); conn.Dispose(); }

            return recordID;
        }

        /// <summary>
        /// Construct Generic INSERT Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <returns>Row ID </returns>
        public int INSERT(string tableName, Dictionary<string, object> columnsValues)
        {
            StringBuilder fields = new StringBuilder(); fields.Append("(");
            StringBuilder values = new StringBuilder(); values.Append("(");
            StringBuilder whereStatement = new StringBuilder();


            //Fields and values
            foreach (KeyValuePair<string, object> pair in columnsValues)
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(DateTime) && (DateTime)pair.Value == DateTime.MinValue)
                {
                    continue;
                }
                else if (
                    pair.Value == DBNull.Value ||
                    pair.Value.ToString() == string.Empty ||
                    pair.Value == null)
                {
                    continue;
                }
                else
                    fields.Append("[" + pair.Key + "],");

                if (valueType == typeof(int) || valueType == typeof(Double) || valueType == typeof(Decimal))
                {
                    values.Append(pair.Value + ",");
                }
                else if (valueType == typeof(DateTime) && (DateTime)pair.Value == DateTime.MinValue)
                {
                    continue;
                }
                else
                {
                    values.Append("'" + pair.Value.ToString().Replace("'", "`") + "'" + ",");
                }
            }

            fields.Remove(fields.Length - 1, 1).Append(")");
            values.Remove(values.Length - 1, 1).Append(")");

            string insertQuery = string.Format("INSERT INTO [{0}] {1} VALUES {2}", tableName, fields, values);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            int recordID = 0;
            try
            {
                conn.Open();
                recordID = Convert.ToInt32(comm.ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); conn.Dispose(); }

            return recordID;
        }

        /// <summary>
        /// Construct Generic INSERT Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <returns>Row ID </returns>
        public int INSERT(string tableName, Dictionary<string, object> columnsValues, string idFieldName)
        {
            StringBuilder fields = new StringBuilder(); fields.Append("(");
            StringBuilder values = new StringBuilder(); values.Append("(");
            StringBuilder whereStatement = new StringBuilder();

            //Fields and values
            foreach (KeyValuePair<string, object> pair in columnsValues)
            {
                fields.Append("[" + pair.Key + "],");

                if (pair.Value == null)
                {
                    values.Append("NULL" + ",");
                }
                else if (pair.Value is int || pair.Value is Double)
                {
                    values.Append(pair.Value + ",");
                }
                else if (pair.Value is DateTime && (DateTime)pair.Value == DateTime.MinValue)
                {
                    continue;
                }
                else
                {
                    values.Append("'" + pair.Value.ToString().Replace("'", "`") + "'" + ",");
                }
            }

            fields.Remove(fields.Length - 1, 1).Append(")");
            values.Remove(values.Length - 1, 1).Append(")");

            string insertQuery = string.Format("INSERT INTO [{0}] {1} OUTPUT INSERTED.{2}  VALUES {3}", tableName, fields, idFieldName, values);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            int recordID = 0;
            try
            {
                conn.Open();
                recordID = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

            return recordID;
        }

        /// <summary>
        /// Update by EXECUTING NATIVE SQL QUERY
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public bool UPDATE(string SQL) 
        {
            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(SQL, conn);
            comm.CommandTimeout = 360;

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }
        }

        /// <summary>
        /// Construct Generic UPDATE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <param name="idFieldName">ID Field name </param>
        /// <param name="ID">ID Value</param>
        /// <returns>Row ID</returns>
        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues, string idFieldName, Int64 ID)
        {
            StringBuilder fieldsValues = new StringBuilder();

            foreach (KeyValuePair<string, object> pair in columnsValues)
            {

                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                else if (valueType == typeof(DateTime) && (DateTime)pair.Value == DateTime.MinValue)
                    continue;
                else
                    fieldsValues.Append("[" + pair.Key + "]=" + "'" + pair.Value + "'" + ",");
            }

            fieldsValues.Remove(fieldsValues.Length - 1, 1);

            string insertQuery = string.Format("UPDATE  [{0}] SET {1} WHERE [{2}]={3}", tableName, fieldsValues, idFieldName, ID);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);
            comm.CommandTimeout = 360;

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

        }

        /// <summary>
        /// Construct Generic UPDATE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <param name="wherePart">Dictionary Holds Fields and Values to be able to construct Where Statemnet</param>
        /// <returns></returns>
        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues, Dictionary<string, object> wherePart)
        {
            StringBuilder fieldsValues = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();

            foreach (KeyValuePair<string, object> columnValuePair in columnsValues)
            {
                if (columnValuePair.Value == null)
                {
                    fieldsValues.Append("[" + columnValuePair.Key + "]=null,");
                }
                else if (columnValuePair.Value is int || columnValuePair.Value is Double)
                {
                    fieldsValues.Append("[" + columnValuePair.Key + "]=" + columnValuePair.Value + ",");
                }
                else if (columnValuePair.Value is bool)
                {
                    if ((bool)columnValuePair.Value == true)
                        fieldsValues.Append("[" + columnValuePair.Key + "]=" + 1 + ",");
                    else
                        fieldsValues.Append("[" + columnValuePair.Key + "]=" + 0 + ",");
                }
                else if (columnValuePair.Value is DateTime && ((DateTime)columnValuePair.Value == DateTime.MinValue))
                {
                    continue;
                }
                else
                {
                    fieldsValues.Append("[" + columnValuePair.Key + "]=" + "'" + columnValuePair.Value.ToString().Replace("'", "`") + "'" + ",");
                }
            }

            fieldsValues.Remove(fieldsValues.Length - 1, 1);

            foreach (KeyValuePair<string, object> wherePair in wherePart)
            {
                if (wherePair.Value is DateTime && ((DateTime)wherePair.Value == DateTime.MinValue))
                {
                    continue;
                }
                else if (wherePair.Value is int || wherePair.Value is Double)
                {
                    whereStatement.Append("[" + wherePair.Key + "]=" + wherePair.Value + " AND ");
                }
                else if (wherePair.Value is bool)
                {
                    if ((bool)wherePair.Value == true)
                        whereStatement.Append("[" + wherePair.Key + "]=" + 1 + " AND ");
                    else
                        whereStatement.Append("[" + wherePair.Key + "]=" + 0 + " AND ");
                }
                else
                {
                    whereStatement.Append("[" + wherePair.Key + "]='" + wherePair.Value.ToString().Replace("'", "`") + "' AND ");
                }

            }
            whereStatement.Remove(whereStatement.Length - 5, 5);

            string insertQuery = string.Format("UPDATE  [{0}] SET {1} WHERE {2}", tableName, fieldsValues, whereStatement);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(insertQuery, conn);
            comm.CommandTimeout = 360;

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }

        }

        /// <summary>
        /// Construct Generic UPDATE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <param name="idFieldName">ID Field name </param>
        /// <param name="ID">ID Value</param>
        /// <returns>Row ID</returns>
        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues, string idFieldName, Int64 ID, ref OleDbConnection conn)
        {
            StringBuilder fieldsValues = new StringBuilder();

            foreach (KeyValuePair<string, object> pair in columnsValues)
            {

                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                else if (valueType == typeof(DateTime) && (DateTime)pair.Value == DateTime.MinValue)
                    continue;
                else
                    fieldsValues.Append("[" + pair.Key + "]=" + "'" + pair.Value + "'" + ",");
            }

            fieldsValues.Remove(fieldsValues.Length - 1, 1);

            string insertQuery = string.Format("UPDATE  [{0}] SET {1} WHERE [{2}]={3}", tableName, fieldsValues, idFieldName, ID);

            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            try
            {
                //if (conn.State == ConnectionState.Closed)
                //    conn.Open();

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }

        }

        /// <summary>
        /// Construct Generic UPDATE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <param name="wherePart">Dictionary Holds Fields and Values to be able to construct Where Statemnet</param>
        /// <returns></returns>
        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues, Dictionary<string, object> wherePart, ref OleDbConnection conn)
        {
            string updateQuery = string.Empty;
            StringBuilder fieldsValues = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();

            if (columnsValues.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in columnsValues)
                {
                    Type valueType = pair.Value.GetType();

                    if (valueType == typeof(int) || valueType == typeof(Double))
                        fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                    else if (valueType == typeof(DateTime) && ((DateTime)pair.Value == DateTime.MinValue))
                        continue;
                    else
                        fieldsValues.Append("[" + pair.Key + "]=" + "'" + pair.Value.ToString().Replace("'", "`") + "'" + ",");
                }

                fieldsValues.Remove(fieldsValues.Length - 1, 1);
            }

            if (wherePart.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in wherePart)
                {
                    Type valueType = pair.Value.GetType();

                    if (valueType == typeof(int) || valueType == typeof(Double))
                        whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                    else if (valueType == typeof(DateTime) && ((DateTime)pair.Value == DateTime.MinValue))
                        continue;
                    else
                        whereStatement.Append("[" + pair.Key + "]='" + pair.Value.ToString().Replace("'", "`") + "' AND ");
                }

                whereStatement.Remove(whereStatement.Length - 5, 5);
            }

            if (whereStatement.Length > 0)
                updateQuery = string.Format("UPDATE  [{0}] SET {1} WHERE {2}", tableName, fieldsValues, whereStatement);
            else
                updateQuery = string.Format("UPDATE  [{0}] SET {1}", tableName, fieldsValues);


            OleDbCommand comm = new OleDbCommand(updateQuery, conn);

            try
            {
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }

        }

        /// <summary>
        /// Construct Generic UPDATE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="columnsValues">Dictionary Holds Fields and Values to be inserted</param>
        /// <param name="wherePart">Dictionary Holds Fields and Values to be able to construct Where Statemnet</param>
        /// <returns></returns>
        public bool UPDATE(string tableName, Dictionary<string, object> columnsValues, ref OleDbConnection conn)
        {
            StringBuilder fieldsValues = new StringBuilder();
            StringBuilder whereStatement = new StringBuilder();

            if (columnsValues.Count < 0)
                return false;

            foreach (KeyValuePair<string, object> pair in columnsValues)
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    fieldsValues.Append("[" + pair.Key + "]=" + pair.Value + ",");
                else if (valueType == typeof(DateTime) && ((DateTime)pair.Value == DateTime.MinValue))
                    continue;
                else
                    fieldsValues.Append("[" + pair.Key + "]=" + "'" + pair.Value.ToString().Replace("'", "`") + "'" + ",");
            }

            fieldsValues.Remove(fieldsValues.Length - 1, 1);



            whereStatement.Append(String.Format("SessionIdTime='{0}' AND SessionIdSeq={1}", columnsValues["SessionIdTime"], columnsValues["SessionIdSeq"]));


            string insertQuery = string.Format("UPDATE  [{0}] SET {1} WHERE {2}", tableName, fieldsValues, whereStatement);

            OleDbCommand comm = new OleDbCommand(insertQuery, conn);

            try
            {
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
        }

        /// <summary>
        /// Delete by EXECUTING NATIVE SQL QUERY
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public bool DELETE(string SQL) 
        {
            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(SQL, conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }
        }
        /// <summary>
        /// Construct Generic DELETE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="idFieldName">ID Field Name</param>
        /// <param name="ID">ID Field Value</param>
        /// <returns>True if Row has been deleted. </returns>
        public bool DELETE(string tableName, string idFieldName, int ID)
        {
            string deleteQuery = string.Format("DELETE FROM [{0}] WHERE [{1}]={2}", tableName, idFieldName, ID);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(deleteQuery, conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }
        }

        /// <summary>
        /// Construct Generic DELETE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="idFieldName">ID Field Name</param>
        /// <param name="ID">ID Field Value</param>
        /// <returns>True if Row has been deleted. </returns>
        public bool DELETE(string tableName, string idFieldName, long ID)
        {
            string deleteQuery = string.Format("DELETE FROM [{0}] WHERE [{1}]={2}", tableName, idFieldName, ID);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(deleteQuery, conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }
        }

        /// <summary>
        /// Construct Generic DELETE Statement
        /// </summary>
        /// <param name="tableName">DB Table Name</param>
        /// <param name="wherePart">Dictionary Holds Fields and Values to be able to construct Where Statemnet</param>
        /// <returns>True if Row has been deleted.</returns>
        public bool DELETE(string tableName, Dictionary<string, object> wherePart)
        {
            StringBuilder whereStatement = new StringBuilder();

            foreach (KeyValuePair<string, object> pair in wherePart)
            {
                Type valueType = pair.Value.GetType();

                if (valueType == typeof(int) || valueType == typeof(Double))
                    whereStatement.Append("[" + pair.Key + "]=" + pair.Value + " AND ");
                else
                    whereStatement.Append("[" + pair.Key + "]='" + pair.Value + "' AND ");

            }
            whereStatement.Remove(whereStatement.Length - 5, 5);

            string deleteQuery = string.Format("DELETE FROM [{0}] WHERE {1}", tableName, whereStatement);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(deleteQuery, conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }
        }

        public bool CREATE_RATES_TABLE(string tablename)
        {
            string createTableQuery = string.Format(
                "CREATE TABLE [dbo].[{0}] " +
                    "(" +
                    " [country_code_dialing_prefix] [bigint] NOT NULL," +
                    " [rate] [decimal](18, 4) NOT NULL," +
                    " CONSTRAINT [{0}] PRIMARY KEY NONCLUSTERED " +
                    " ([country_code_dialing_prefix] ASC )" +
                    " WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]",
                tablename);

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(createTableQuery.ToString(), conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }
        }

        public bool CREATE(string tableName, Dictionary<string, string> columns)
        {


            StringBuilder createTableQuery = new StringBuilder();
            createTableQuery.Append("CREATE TABLE ");
            createTableQuery.Append(tableName);
            createTableQuery.Append(" ( ");

            foreach (KeyValuePair<string, string> keyValue in columns)
            {
                createTableQuery.Append(keyValue.Key);
                createTableQuery.Append(" ");
                createTableQuery.Append(keyValue.Value);
                createTableQuery.Append(", ");
            }

            createTableQuery.Length -= 2;   //Remove trailing ", "
            createTableQuery.Append(")");

            OleDbConnection conn = DBInitializeConnection(ConnectionString_Lync);
            OleDbCommand comm = new OleDbCommand(createTableQuery.ToString(), conn);

            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Exception", "ex", ex);
                throw argEx;
            }
            finally { conn.Close(); }


        }

        public OleDbDataReader EXECUTEREADER(string sqlStatment, OleDbConnection connection)
        {
            OleDbCommand command;

            command = new OleDbCommand(sqlStatment, connection);
            command.CommandTimeout = 20000;

            return command.ExecuteReader();
        }

        private static string GetDateForDatabase(DateTime dt)
        {
            return dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "." + dt.Millisecond;
        }

        public void OpenConnection(ref OleDbConnection conn)
        {
            conn.Open();
        }

        public void CloseConnection(ref OleDbConnection conn)
        {
            conn.Close();
        }

    }
}
