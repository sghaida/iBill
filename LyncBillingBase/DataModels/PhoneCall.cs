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

        [IsIDField]
        [DbColumn("SessionIdTime")] 
        public string SessionIdTime { set; get; }

        [DbColumn("SessionIdSeq")] 
        public int SessionIdSeq { get; set; }

        [DbColumn("ResponseTime")] 
        public string ResponseTime { set; get; }

        [DbColumn("SessionEndTime")] 
        public string SessionEndTime { set; get; }

        [DbColumn("ChargingParty")] 
        public string ChargingParty { set; get; }

        [DbColumn("DestinationNumberUri")] 
        public string DestinationNumberUri { set; get; }

        [DbColumn("Marker_CallToCountry")] 
        public string Marker_CallToCountry { set; get; }

        [DbColumn("Marker_CallType")] 
        public string Marker_CallType { set; get; }

        [DbColumn("Duration")] 
        public decimal Duration { set; get; }

        [DbColumn("UI_UpdatedByUser")] 
        public string UI_UpdatedByUser { set; get; }

        [DbColumn("UI_MarkedOn")] 
        public string UI_MarkedOn { set; get; }

        [DbColumn("UI_CallType")] 
        public string UI_CallType { set; get; }

        [DbColumn("UI_AssignedByUser")] 
        public string UI_AssignedByUser { set; get; }

        [DbColumn("UI_AssignedToUser")] 
        public string UI_AssignedToUser { set; get; }

        [DbColumn("UI_AssignedOn")] 
        public DateTime UI_AssignedOn { set; get; }

        [DbColumn("AC_DisputeStatus")] 
        public string AC_DisputeStatus { set; get; }

        [DbColumn("AC_DisputeResolvedOn")] 
        public DateTime AC_DisputeResolvedOn { set; get; }

        [DbColumn("AC_IsInvoiced")] 
        public string AC_IsInvoiced { set; get; }

        [DbColumn("AC_InvoiceDate")] 
        public DateTime AC_InvoiceDate { set; get; }

        [DbColumn("SourceUserUri")] 
        public string SourceUserUri { set; get; }

        [DbColumn("SourceNumberUri")] 
        public string SourceNumberUri { set; get; }

        [DbColumn("DestinationUserUri")] 
        public string DestinationUserUri { get; set; }

        [DbColumn("FromMediationServer")] 
        public string FromMediationServer { set; get; }

        [DbColumn("ToMediationServer")] 
        public string ToMediationServer { set; get; }

        [DbColumn("FromGateway")] 
        public string FromGateway { set; get; }

        [DbColumn("ToGateway")] 
        public string ToGateway { set; get; }

        [DbColumn("SourceUserEdgeServer")] 
        public string SourceUserEdgeServer { set; get; }

        [DbColumn("DestinationUserEdgeServer")] 
        public string DestinationUserEdgeServer { set; get; }

        [DbColumn("ServerFQDN")] 
        public string ServerFQDN { set; get; }

        [DbColumn("PoolFQDN")] 
        public string PoolFQDN { set; get; }

        [DbColumn("OnBehalf")] 
        public string OnBehalf { set; get; }

        [DbColumn("ReferredBy")] 
        public string ReferredBy { set; get; }

        [DbColumn("CalleeURI")] 
        public string CalleeURI { get; set; }

        [DbColumn("Marker_CallFrom")] 
        public long Marker_CallFrom { set; get; }

        [DbColumn("Marker_CallTo")] 
        public long Marker_CallTo { set; get; }

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
