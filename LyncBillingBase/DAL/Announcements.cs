using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class Announcements
    {
        int ID { get; set; }
        string Announcement { get; set; }
        string ForRole { get; set; }
        string ForSite { get; set; }
        DateTime PublishOn { get; set; } 
    }
}
