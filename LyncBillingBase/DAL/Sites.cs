using LyncBillingBase.LIBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.HELPERS;

namespace LyncBillingBase.DAL
{
    public class Sites
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }

        //This is a logical representation of data, it doesn't belong to the table.
        public string CountryName { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<Sites> GetAllSites(List<string> columns = null, Dictionary<string, object> wherePart = null, int limits = 0)
        {
            Sites site;
            DataTable dt = new DataTable();
            List<Sites> sites = new List<Sites>();


            //Exception-handling
            try
            {
                dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Sites.TableName), columns, wherePart, limits);

              

                return sites.OrderBy(item => item.SiteName).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
