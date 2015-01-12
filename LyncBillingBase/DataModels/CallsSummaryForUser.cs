using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.DistributedSource)]
    public class CallsSummaryForUser : DataModel
    {
        [IsIDField]
        [DbColumn("Date")]
        public DateTime Date { get; set; }

        [DbColumn("Year")]
        public int Year { get; set; }

        [DbColumn("Month")]
        public int Month { get; set; }

        [DbColumn("ChargingParty")]
        public string SipAccount { get; set; }

        [DbColumn("AC_IsInvoiced")]
        public string IsInvoiced { get; set; }

        // Business
        [DbColumn("BusinessCallsCount")]
        public long BusinessCallsCount { get; set; }

        [DbColumn("BusinessCallsDuration")]
        public long BusinessCallsDuration { get; set; }

        [DbColumn("BusinessCallsCost")]
        public decimal BusinessCallsCost { get; set; }

        // Personal
        [DbColumn("PersonalCallsCount")]
        public long PersonalCallsCount { get; set; }

        [DbColumn("PersonalCallsDuration")]
        public long PersonalCallsDuration { get; set; }

        [DbColumn("PersonalCallsCost")]
        public decimal PersonalCallsCost { get; set; }

        // Unmarked
        [DbColumn("UnmarkedCallsCount")]
        public long UnmarkedCallsCount { get; set; }

        [DbColumn("UnmarkedCallsDuration")]
        public long UnmarkedCallsDuration { get; set; }

        [DbColumn("UnmarkedCallsCost")]
        public decimal UnmarkedCallsCost { get; set; }


        public decimal TotalCallsCost 
        {
            get { return (this.BusinessCallsCost + this.PersonalCallsCost + this.UnmarkedCallsCost); }
        }

        public long TotalCallsDuration
        {
            get { return (this.BusinessCallsDuration + this.PersonalCallsDuration + this.UnmarkedCallsDuration); }
        }

        public long TotalCallsCount
        {
            get { return (this.BusinessCallsCount + this.PersonalCallsCount + this.UnmarkedCallsCount); }
        }



        //
        // Relations
        [DataRelation(WithDataModel = typeof(User), OnDataModelKey = "SipAccount", ThisKey = "SipAccount")]
        public User User { get; set; }
    }

}
