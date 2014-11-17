using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;


namespace LyncBillingBase.DAL
{ 
    public class PhoneCalls
    {
        public PhoneCalls() { }

        private static DBLib DBRoutines = new DBLib();

        //Common and wanted 
        public string ChargingParty { set; get; }
        public string SessionIdTime { set; get; }
        public string DestinationNumberUri { set; get; }
        public string Marker_CallToCountry { set; get; }
        public string Marker_CallType { set; get; }
        public decimal Duration { set; get; }

        public string UI_UpdatedByUser { set; get; }
        public string UI_MarkedOn { set; get; }
        public string UI_CallType { set; get; }
        public string UI_AssignedByUser { set; get; }
        public string UI_AssignedToUser { set; get; }
        public DateTime UI_AssignedOn { set; get; }

        public string AC_DisputeStatus { set; get; }
        public DateTime AC_DisputeResolvedOn { set; get; }
        public string AC_IsInvoiced { set; get; }
        public DateTime AC_InvoiceDate { set; get; }

        /***
         * FRONTEND UNWATED
         */
        //public int SessionIdSeq { get; set; }
        //public string ResponseTime { set; get; }
        //public string SessionEndTime { set; get; }
        //public string SourceUserUri { set; get; }
        //public string SourceNumberUri { set; get; }
        //public string DestinationUserUri { get; set; }
        //public string FromMediationServer { set; get; }
        //public string ToMediationServer { set; get; }
        //public string FromGateway { set; get; }
        //public string ToGateway { set; get; }
        //public string SourceUserEdgeServer { set; get; }
        //public string DestinationUserEdgeServer { set; get; }
        //public string ServerFQDN { set; get; }
        //public string PoolFQDN { set; get; }
        //public string OnBehalf { set; get; }
        //public string ReferredBy { set; get; }
        //public string CalleeURI { get; set; }
        //public long Marker_CallFrom { set; get; }
        //public long Marker_CallTo { set; get; }
        //public int Marker_CallTypeID { set; get; }

        /***
         * BACKEND UNWANTED
         */
        //public string PhoneBookName { set; get; }

        //This is used to tell where to find and update this PhoneCalls
        //public string PhoneCallsTableName { get; set; }

        private decimal _Marker_CallCost;

        public decimal Marker_CallCost
        {
            set { this._Marker_CallCost = value; }
            get { return decimal.Round(this._Marker_CallCost, 2); }
        }

        public static List<PhoneCalls> GetPhoneCalls(string primarySipAccount, List<string> bundledAccounts = null, Dictionary<string, object> wherePart = null, int limits = 0)
        {
            DataTable dt;
            List<PhoneCalls> phoneCalls;

            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_ChargeableCalls_ForUser);
            //PhoneCallsComparer linqDistinctComparer = new PhoneCallsComparer();

            //Initialize function parameters and then query the database
            List<object> functionaParams = new List<object>() { primarySipAccount };

            try
            {
                dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionaParams, wherePart);
                phoneCalls = dt.ToList<PhoneCalls>();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (limits > 0)
                return phoneCalls.GetRange(0, limits);
            else
                return phoneCalls;
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
