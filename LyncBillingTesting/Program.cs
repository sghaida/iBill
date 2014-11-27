using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;
using LyncBillingBase.Helpers;
using LyncBillingBase.Repository;



namespace LyncBillingTesting
{
    class Program
    {
        public  static string tolower(string text)
        {
            return text.ToLower();
        }

        public static void Main(string[] args)
        {
            //var _dbStorage = DataStorage.Instance;

            //Expression<Func<PhoneCall, bool>> expr = (item) => item.ChargingParty == "sghaida@ccc.gr" as string && item.SourceUserUri=="sghaida@ccc.gr";
            
            //List<PhoneCall> phoneCalls = _dbStorage.PhoneCalls.GetChargableCallsPerUser("sghaida@ccc.gr").ToList();

            //List<PhoneCall> siteCalls = _dbStorage.PhoneCalls.GetChargeableCallsForSite("moa").ToList();

            //List<Site> allSites = _dbStorage.SitesDepartments.Sites.GetAll().ToList<Site>();
            //List<Department> allDepartments = _dbStorage.SitesDepartments.Departments.GetAll().ToList<Department>();

            //Site MOA = allSites.Find(site => site.Name == "MOA");

            //List<SiteDepartment> siteDepartments = _dbStorage.SitesDepartments.GetAll().ToList();

            //var siteDepartmentsWithEmptyNames = siteDepartments.Where(item => string.IsNullOrEmpty(item.DepartmentName) == true).ToList();

            DataAccess<SiteDepartment> sitesDepartements = new DataAccess<SiteDepartment>();

            var allData = sitesDepartements.GetAllWithRelations();
        }
    }
}
