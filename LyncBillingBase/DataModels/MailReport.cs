using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MailStatistics", Type = Globals.DataSource.Type.DbTable, AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class MailReport : DataModel
    {
        [IsIdField]
        [DbColumn("id")]
        public int Id { get; set; }

        [DbColumn("EmailAddress")]
        public string EmailAddress { get; set; }

        [DbColumn("RecievedCount")]
        public long ReceivedCount { get; set; }

        [DbColumn("RecievedSize")]
        public long ReceivedSize { get; set; }

        [DbColumn("SentCount")]
        public long SentCount { get; set; }

        [DbColumn("SentSize")]
        public long SentSize { get; set; }

        [DbColumn("TimeStamp")]
        public DateTime ReportDate { get; set; }
    }
}
