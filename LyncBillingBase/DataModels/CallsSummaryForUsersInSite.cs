using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "", Type = GLOBALS.DataSource.Type.DBTable,
        AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    internal class CallsSummaryForUsersInSite : DataModel
    {
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

        [DbColumn("BusinessCallsCount")]
        public int BusinessCallsCount { get; set; }

        [DbColumn("BusinessCallsCost")]
        public decimal BusinessCallsCost { get; set; }

        [DbColumn("BusinessCallsDuration")]
        public int BusinessCallsDuration { get; set; }

        [DbColumn("PersonalCallsCount")]
        public int PersonalCallsCount { get; set; }

        [DbColumn("PersonalCallsDuration")]
        public int PersonalCallsDuration { get; set; }

        [DbColumn("PersonalCallsCost")]
        public decimal PersonalCallsCost { get; set; }

        [DbColumn("UnmarkedCallsCount")]
        public int UnmarkedCallsCount { get; set; }

        [DbColumn("UnmarkedCallsDuration")]
        public int UnmarkedCallsDuration { get; set; }

        [DbColumn("UnmarkedCallsCost")]
        public decimal UnmarkedCallsCost { get; set; }

        public decimal TotalCallsCost
        {
            get { return (BusinessCallsCost + PersonalCallsCost + UnmarkedCallsCost); }
        }

        public long TotalCallsDuration
        {
            get { return (BusinessCallsDuration + PersonalCallsDuration + UnmarkedCallsDuration); }
        }

        public long TotalCallsCount
        {
            get { return (BusinessCallsCount + PersonalCallsCount + UnmarkedCallsCount); }
        }

        //
        // Relations
        [DataRelation(WithDataModel = typeof (User), OnDataModelKey = "SipAccount", ThisKey = "SipAccount")]
        public User User { get; set; }
    }
}