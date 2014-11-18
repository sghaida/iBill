using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    public class Sites
    {
        [DbColumnAttribute("SiteID")]
        public int SiteID { get; set; }

        [DbColumnAttribute("SiteName")]
        public string SiteName { get; set; }

        [DbColumnAttribute("CountryCode")]
        public string CountryCode { get; set; }

        [DbColumnAttribute("Description")]
        public string Description { get; set; }

        public string CountryName { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<Sites> GetAllSites(List<string> columns = null, Dictionary<string, object> wherePart = null, int limits = 0)
        {
            DataTable dt = new DataTable();
            List<Sites> sites = new List<Sites>();

            //Exception-handling
            try
            {
                sites = dt.ToList<Sites>();

                if(sites.Count > 0)
                    sites = sites.OrderBy(item => item.SiteName).ToList();

                return sites;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
