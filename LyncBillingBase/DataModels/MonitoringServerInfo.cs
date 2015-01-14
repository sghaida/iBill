using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class MonitoringServerInfo : DataModel
    {
        [IsIdField]
        [DbColumn("id")]
        public int Id { get; set; }

        [DbColumn("instanceHostName")]
        public string InstanceHostName { get; set; }

        [DbColumn("instanceName")]
        public string InstanceName { get; set; }

        [DbColumn("databaseName")]
        public string DatabaseName { get; set; }

        [DbColumn("userName")]
        public string Username { get; set; }

        [DbColumn("password")]
        public string Password { get; set; }

        [DbColumn("TelephonySolutionName")]
        public string TelephonySolutionName { get; set; }

        [DbColumn("phoneCallsTable")]
        public string PhoneCallsTable { get; set; }

        [DbColumn("description")]
        public string Description { get; set; }

        [AllowNull]
        [DbColumn("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}