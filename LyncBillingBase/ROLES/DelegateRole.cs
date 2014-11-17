using LyncBillingBase.DAL;
using LyncBillingBase.LIBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.HELPERS;

namespace LyncBillingBase.ROLES
{
    public class DelegateRole
    {
        public int ID { set; get; }
        public int DelegeeType { get; set; }
        public int SiteID { get; set; }
        public int DepartmentID { get; set; }
        public string SipAccount { get; set; }
        public string DelegeeSipAccount { get; set; }
        public string Description { get; set; }

        //These are logical representation of data, they don't belong to the table
        public string DelegeeDisplayName { get; set; }
        public Users DelegeeUser { get; set; }
        public Sites DelegeeSite { get; set; }
        public SitesDepartments DelegeeDepartment { get; set; }

        //The following are also logical representation of data, they are used to quickly lookup names from the Ext.NET views
        public string DelegeeDepartmentName { get; set; }
        public string DelegeeSiteName { get; set; }

        //These are for lookup use only in the application
        public static int UserDelegeeTypeID { get { return Convert.ToInt32(Enums.GetValue(Enums.DelegateTypes.UserDelegeeType)); } }
        public static int DepartmentDelegeeTypeID { get { return Convert.ToInt32(Enums.GetValue(Enums.DelegateTypes.DepartemntDelegeeType)); ; } }
        public static int SiteDelegeeTypeID { get { return Convert.ToInt32(Enums.GetValue(Enums.DelegateTypes.SiteDelegeeType)); ; } }

        private static DBLib DBRoutines = new DBLib();

        public static bool IsUserDelegate(string delegateAccount)
        {
            try
            {
                List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.UserDelegeeTypeID);
                return (delegatedAccounts.Count > 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool IsDepartmentDelegate(string delegateAccount)
        {
            try
            {
                List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.DepartmentDelegeeTypeID);
                return (delegatedAccounts.Count > 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool IsSiteDelegate(string delegateAccount)
        {
            try
            {
                List<DelegateRole> delegatedAccounts = GetDelegees(delegateAccount, DelegateRole.SiteDelegeeTypeID);
                return (delegatedAccounts.Count > 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<DelegateRole> GetDelegees(string delegateSipAccount = null, int? delegeeType = null)
        {
            DataTable dt = new DataTable();

    
            List<DelegateRole> DelegatedAccounts = new List<DelegateRole>();
           

            return DelegatedAccounts;
        }
        


    }
}
