using FastMember;
using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;
using LyncBillingBase.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lync2013Plugin.Implementation
{
   
    public static class DB
    {
        private static DBLib DBRoutines = new DBLib();

        //Define what attributes to be read from the class
        private const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;


        public static void BulkInsert(this List<PhoneCall> source, string tableName) 
        {
            List<PropertyInfo> masterPropertyInfoFields = new List<PropertyInfo>();
           
            //SQL Bulk Copy only works with sql connection so we need to remove the provider from the connection string
            using (var bcp = new SqlBulkCopy(DBLib.ConnectionString_Lync.Replace(@"Provider=SQLOLEDB.1;",""))) 
            {
                var AllProperties =  source.GetType().GetGenericArguments().Single().GetProperties(flags).
                    Where(property => property.GetCustomAttribute<DbColumnAttribute>() != null).
                    Cast<PropertyInfo>().Select(item => item.GetCustomAttribute<DbColumnAttribute>().Name).ToArray();

                masterPropertyInfoFields = source.GetType().GetGenericArguments().Single().GetProperties(flags)
               .Where(property => 
                    property.GetCustomAttribute<DbColumnAttribute>() != null &&  
                    property.Name != "PhoneCallsTableName" && 
                    property.Name != "PhoneBookName" && 
                    property.Name != "PhoneCallsTable") 
               .Cast<PropertyInfo>()
               .ToList();

                using (var reader = ObjectReader.Create<PhoneCall>(source, AllProperties))
                {
                    bcp.DestinationTableName = tableName;
                    
                    foreach (var item in masterPropertyInfoFields)
                    {   
                        bcp.ColumnMappings.Add(new SqlBulkCopyColumnMapping(item.Name, item.GetCustomAttribute<DbColumnAttribute>().Name));
                    }
                    
                    bcp.WriteToServer(reader);
                }
            }
        }

        
        public static void BulkInsert(this DataTable dt, string tableName) 
        {
            try
            {
              
                using (SqlBulkCopy bcp = new SqlBulkCopy(DBLib.ConnectionString_Lync.Replace(@"Provider=SQLOLEDB.1;", "")
                    , SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.KeepIdentity)) 
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        bcp.ColumnMappings.Add(new SqlBulkCopyColumnMapping(dc.ColumnName, dc.ColumnName));
                    }

                    bcp.BulkCopyTimeout = 120;
                    bcp.BatchSize = 1000;
                    bcp.DestinationTableName = tableName;
                    bcp.WriteToServer(dt.Select());
                }
            }
            catch (Exception e) 
            {
                throw e.InnerException;
            }
        }

        
        public static IEnumerable<T> ReadSqlData<T>(OleDbDataReader reader, Func<IDataRecord, T> selector)
        {
            while (reader.Read()) 
            {
                yield return selector(reader);
            }
        }


        public static Func<IDataRecord, PhoneCall> PhoneCallsSelector = (record) => new PhoneCall
        {
            PhoneCallsTableName = "PhoneCalls2013",

            SessionIdTime = Helpers.IsNull(record["SessionIdTime"]) ? DateTime.MinValue : record.GetDateTime(record.GetOrdinal("SessionIdTime")),
            SessionIdSeq = Helpers.IsNull(record["SessionIdSeq"]) ? Convert.ToInt32(0) : record.GetInt32(record.GetOrdinal("SessionIdSeq")),
            ResponseTime = Helpers.IsNull(record["ResponseTime"]) ? DateTime.MinValue : record.GetDateTime(record.GetOrdinal("ResponseTime")),
            SessionEndTime = Helpers.IsNull(record["SessionEndTime"]) ? DateTime.MinValue : record.GetDateTime(record.GetOrdinal("SessionEndTime")),

            Duration = Helpers.IsNull(record["Duration"]) ? Convert.ToDecimal(0) : record.GetDecimal(record.GetOrdinal("Duration")),

            SourceUserUri = Helpers.IsNull(record["SourceUserUri"]) ? string.Empty : record.GetString(record.GetOrdinal("SourceUserUri")),
            DestinationUserUri = Helpers.IsNull("DestinationUserUri") ? string.Empty : record.GetString(record.GetOrdinal("DestinationUserUri")),

            SourceNumberUri = Helpers.IsNull(record["SourceNumberUri"]) ? string.Empty : record.GetString(record.GetOrdinal("SourceNumberUri")),
            DestinationNumberUri = Helpers.IsNull(record["DestinationNumberUri"]) ? string.Empty : record.GetString(record.GetOrdinal("DestinationNumberUri")),

            FromMediationServer = Helpers.IsNull(record["FromMediationServer"]) ? string.Empty : record.GetString(record.GetOrdinal("FromMediationServer")),
            ToMediationServer = Helpers.IsNull(record["ToMediationServer"]) ? string.Empty : record.GetString(record.GetOrdinal("ToMediationServer")),

            FromGateway = Helpers.IsNull(record["FromGateway"]) ? string.Empty : record.GetString(record.GetOrdinal("FromGateway")),
            ToGateway = Helpers.IsNull(record["ToGateway"]) ? string.Empty : record.GetString(record.GetOrdinal("ToGateway")),

            SourceUserEdgeServer = Helpers.IsNull(record["SourceUserEdgeServer"]) ? string.Empty : record.GetString(record.GetOrdinal("SourceUserEdgeServer")),
            DestinationUserEdgeServer = Helpers.IsNull(record["DestinationUserEdgeServer"]) ? string.Empty : record.GetString(record.GetOrdinal("DestinationUserEdgeServer")),

            ServerFQDN = Helpers.IsNull(record["ServerFQDN"]) ? string.Empty : record.GetString(record.GetOrdinal("ServerFQDN")),
            PoolFQDN = Helpers.IsNull(record["PoolFQDN"]) ? string.Empty : record.GetString(record.GetOrdinal("PoolFQDN")),
            ReferredBy = Helpers.IsNull(record["ReferredBy"]) ? string.Empty : record.GetString(record.GetOrdinal("ReferredBy")),
            OnBehalf = Helpers.IsNull(record["OnBehalf"]) ? string.Empty : record.GetString(record.GetOrdinal("OnBehalf")),
            CalleeURI = Helpers.IsNull(record["CalleeURI"]) ? string.Empty : record.GetString(record.GetOrdinal("CalleeURI")),
            
            //ChargingParty = Helpers.IsNull(record["ChargingParty"]) ? string.Empty : record.GetString(record.GetOrdinal("ChargingParty")),
            
            //Marker_CallFrom = Helpers.IsNull(record["Marker_CallFrom"]) ? Convert.ToInt64(0) : record.GetInt64(record.GetOrdinal("Marker_CallFrom")),
            //Marker_CallTo = Helpers.IsNull(record["Marker_CallTo"]) ? Convert.ToInt64(0) : record.GetInt64(record.GetOrdinal("Marker_CallTo")),
            //Marker_CallToCountry = Helpers.IsNull(record["Marker_CallToCountry"]) ? string.Empty : record.GetString(record.GetOrdinal("Marker_CallToCountry")),
            //Marker_CallCost = Helpers.IsNull(record["Marker_CallCost"]) ? Convert.ToDecimal(0) : record.GetDecimal(record.GetOrdinal("Marker_CallCost")),
            //Marker_CallTypeID = Helpers.IsNull(record["Marker_CallTypeID"]) ? Convert.ToInt64(0) : record.GetInt64(record.GetOrdinal("Marker_CallTypeID")),
            //Marker_CallType = Helpers.IsNull(record["Marker_CallType"]) ? string.Empty : record.GetString(record.GetOrdinal("Marker_CallType")),

            //UI_MarkedOn = Helpers.IsNull(record["UI_MarkedOn"]) ? DateTime.MinValue : record.GetDateTime(record.GetOrdinal("UI_MarkedOn")),
            //UI_UpdatedByUser = Helpers.IsNull(record["UI_UpdatedByUser"]) ? string.Empty : record.GetString(record.GetOrdinal("UI_UpdatedByUser")),
            //UI_AssignedByUser = Helpers.IsNull(record["UI_AssignedByUser"]) ? string.Empty : record.GetString(record.GetOrdinal("UI_AssignedByUser")),
            //UI_AssignedOn = Helpers.IsNull(record["UI_AssignedOn"]) ? DateTime.MinValue : record.GetDateTime(record.GetOrdinal("UI_AssignedOn")),
            //UI_CallType = Helpers.IsNull(record["UI_CallType"]) ? string.Empty : record.GetString(record.GetOrdinal("UI_CallType")),
            //AC_DisputeStatus = Helpers.IsNull(record["AC_DisputeStatus"]) ? string.Empty : record.GetString(record.GetOrdinal("AC_DisputeStatus")),
            //AC_DisputeResolvedOn = Helpers.IsNull(record["AC_DisputeResolvedOn"]) ? DateTime.MinValue : record.GetDateTime(record.GetOrdinal("AC_DisputeResolvedOn")),
            //AC_IsInvoiced = Helpers.IsNull(record["AC_IsInvoiced"]) ? string.Empty : record.GetString(record.GetOrdinal("AC_IsInvoiced")),
            //AC_InvoiceDate = Helpers.IsNull(record["AC_InvoiceDate"]) ? DateTime.MinValue : record.GetDateTime(record.GetOrdinal("AC_InvoiceDate")),
            //UI_AssignedToUser = Helpers.IsNull(record["UI_AssignedToUser"]) ? string.Empty : record.GetString(record.GetOrdinal("UI_AssignedToUser"))
        };
    }

}
