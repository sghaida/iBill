using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class PhoneCall : DataModel
    {
        //This is used to tell where to find and update this PhoneCalls


        private decimal _markerCallCost;

        [IsIdField]
        [AllowIdInsert]
        [DbColumn("SessionIdTime")]
        public DateTime SessionIdTime { set; get; }

        [DbColumn("SessionIdSeq")]
        public int SessionIdSeq { get; set; }

        [DbColumn("ResponseTime")]
        public DateTime ResponseTime { set; get; }

        [DbColumn("SessionEndTime")]
        public DateTime SessionEndTime { set; get; }

        [AllowNull]
        [DbColumn("ChargingParty")]
        public string ChargingParty { set; get; }

        [AllowNull]
        [DbColumn("DestinationNumberUri")]
        public string DestinationNumberUri { set; get; }

        [AllowNull]
        [DbColumn("Marker_CallToCountry")]
        public string MarkerCallToCountry { set; get; }

        [DbColumn("Marker_CallType")]
        public string MarkerCallType { set; get; }

        [DbColumn("Duration")]
        public decimal Duration { set; get; }

        [AllowNull]
        [DbColumn("UI_UpdatedByUser")]
        public string UiUpdatedByUser { set; get; }

        [AllowNull]
        [DbColumn("UI_MarkedOn")]
        public DateTime UiMarkedOn { set; get; }

        [AllowNull]
        [DbColumn("UI_CallType")]
        public string UiCallType { set; get; }

        [AllowNull]
        [DbColumn("UI_AssignedByUser")]
        public string UiAssignedByUser { set; get; }

        [AllowNull]
        [DbColumn("UI_AssignedToUser")]
        public string UiAssignedToUser { set; get; }

        [AllowNull]
        [DbColumn("UI_AssignedOn")]
        public DateTime UiAssignedOn { set; get; }

        [AllowNull]
        [DbColumn("AC_DisputeStatus")]
        public string AcDisputeStatus { set; get; }

        [AllowNull]
        [DbColumn("AC_DisputeResolvedOn")]
        public DateTime AcDisputeResolvedOn { set; get; }

        [AllowNull]
        [DbColumn("AC_IsInvoiced")]
        public string AcIsInvoiced { set; get; }

        [AllowNull]
        [DbColumn("AC_InvoiceDate")]
        public DateTime AcInvoiceDate { set; get; }

        [DbColumn("SourceUserUri")]
        public string SourceUserUri { set; get; }

        [AllowNull]
        [DbColumn("SourceNumberUri")]
        public string SourceNumberUri { set; get; }

        [DbColumn("DestinationUserUri")]
        public string DestinationUserUri { get; set; }

        [AllowNull]
        [DbColumn("FromMediationServer")]
        public string FromMediationServer { set; get; }

        [AllowNull]
        [DbColumn("ToMediationServer")]
        public string ToMediationServer { set; get; }

        [AllowNull]
        [DbColumn("FromGateway")]
        public string FromGateway { set; get; }

        [AllowNull]
        [DbColumn("ToGateway")]
        public string ToGateway { set; get; }

        [AllowNull]
        [DbColumn("SourceUserEdgeServer")]
        public string SourceUserEdgeServer { set; get; }

        [AllowNull]
        [DbColumn("DestinationUserEdgeServer")]
        public string DestinationUserEdgeServer { set; get; }

        [DbColumn("ServerFQDN")]
        public string ServerFqdn { set; get; }

        [DbColumn("PoolFQDN")]
        public string PoolFqdn { set; get; }

        [AllowNull]
        [DbColumn("OnBehalf")]
        public string OnBehalf { set; get; }

        [AllowNull]
        [DbColumn("ReferredBy")]
        public string ReferredBy { set; get; }

        [AllowNull]
        [DbColumn("CalleeURI")]
        public string CalleeUri { get; set; }

        [AllowNull]
        [DbColumn("Marker_CallFrom")]
        public long MarkerCallFrom { set; get; }

        [AllowNull]
        [DbColumn("Marker_CallTo")]
        public long MarkerCallTo { set; get; }

        [AllowNull]
        [DbColumn("Marker_CallTypeID")]
        public long MarkerCallTypeId { set; get; }

        [DbColumn("PhoneCallsTableName")]
        [Exclude(OnInsert = true, OnUpdate = true)]
        public string PhoneCallsTableName { get; set; }

        public string PhoneBookName { set; get; }
        public string PhoneCallsTable { set; get; }

        [DbColumn("Marker_CallCost")]
        public decimal MarkerCallCost
        {
            set { _markerCallCost = value; }
            get { return decimal.Round(_markerCallCost, 2); }
        }
    }

    //The phonecalls version of the IEqualityComparer, used with LINQ's Distinct function
    //class PhoneCallsComparer : IEqualityComparer<PhoneCalls>
    //{
    //    public bool Equals(PhoneCalls firstCall, PhoneCalls secondCall)
    //    {
    //        return (
    //            firstCall.SourceUserUri == secondCall.SourceUserUri &&
    //            firstCall.SessionIdTime == secondCall.SessionIdTime &&
    //            firstCall.SessionIdSeq == secondCall.SessionIdSeq
    //        );
    //    }

    //    public int GetHashCode(PhoneCalls call)
    //    {
    //        string hashcode = call.SourceUserUri.ToString() + call.SessionIdTime + call.SessionIdSeq;
    //        return hashcode.GetHashCode();
    //    }
    //}
}