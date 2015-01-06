using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.DistributedSource)]
    public class PhoneCall : DataModel
    {
        public PhoneCall() { }

        [IsKey]
        [IsIDField]
        [AllowIDInsert]
        [DbColumn("SessionIdTime")] 
        public DateTime SessionIdTime { set; get; }

        [IsKey]
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
        public string Marker_CallToCountry { set; get; }

        [DbColumn("Marker_CallType")] 
        public string Marker_CallType { set; get; }

        [DbColumn("Duration")] 
        public decimal Duration { set; get; }

        [AllowNull]
        [DbColumn("UI_UpdatedByUser")] 
        public string UI_UpdatedByUser { set; get; }

        [AllowNull]
        [DbColumn("UI_MarkedOn")]
        public DateTime UI_MarkedOn { set; get; }

        [AllowNull]
        [DbColumn("UI_CallType")] 
        public string UI_CallType { set; get; }

        [AllowNull]
        [DbColumn("UI_AssignedByUser")] 
        public string UI_AssignedByUser { set; get; }

        [AllowNull]
        [DbColumn("UI_AssignedToUser")] 
        public string UI_AssignedToUser { set; get; }

        [AllowNull]
        [DbColumn("UI_AssignedOn")] 
        public DateTime UI_AssignedOn { set; get; }

        [AllowNull]
        [DbColumn("AC_DisputeStatus")] 
        public string AC_DisputeStatus { set; get; }

        [AllowNull]
        [DbColumn("AC_DisputeResolvedOn")] 
        public DateTime AC_DisputeResolvedOn { set; get; }

        [AllowNull]
        [DbColumn("AC_IsInvoiced")] 
        public string AC_IsInvoiced { set; get; }

        [AllowNull]
        [DbColumn("AC_InvoiceDate")] 
        public DateTime AC_InvoiceDate { set; get; }

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
        public string ServerFQDN { set; get; }

        [DbColumn("PoolFQDN")] 
        public string PoolFQDN { set; get; }

        [AllowNull]
        [DbColumn("OnBehalf")] 
        public string OnBehalf { set; get; }

        [AllowNull]
        [DbColumn("ReferredBy")] 
        public string ReferredBy { set; get; }

        [AllowNull]
        [DbColumn("CalleeURI")] 
        public string CalleeURI { get; set; }
        
        [AllowNull]
        [DbColumn("Marker_CallFrom")] 
        public long Marker_CallFrom { set; get; }

        [AllowNull]
        [DbColumn("Marker_CallTo")] 
        public long Marker_CallTo { set; get; }

        [AllowNull]
        [DbColumn("Marker_CallTypeID")] 
        public long Marker_CallTypeID { set; get; }

        [DbColumn("PhoneCallsTableName")]
        [Exclude(OnInsert = true, OnUpdate = true)]
        public string PhoneCallsTableName { get; set; }

        public string PhoneBookName { set; get; }

        public string PhoneCallsTable { set; get; }

        //This is used to tell where to find and update this PhoneCalls
        

        private decimal _Marker_CallCost;

        [DbColumn("Marker_CallCost")] 
        public decimal Marker_CallCost
        {
            set { this._Marker_CallCost = value; }
            get { return decimal.Round(this._Marker_CallCost, 2); }
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
