using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class DIDs
    {
        public int id { get; set; }
        public string Regex { get; set; }
        public string description { get; set; }
        public int SiteID { get; set; }
        // to get the site name for the specific DID instead of checking it everynow and then from the database
        public string SiteName { get; set; }
    }
}
