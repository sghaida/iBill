using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LyncBillingBase.HELPERS;

namespace LyncBillingBase.DAL
{

    [TableName("Announcements")]
    public class Announcements
    {
        
        int ID { get; set; }
        
        [IsAllowNull(true)]
        string Announcement { get; set; }
        
        [IsAllowNull(true)]
        string ForRole { get; set; }
        
        [IsAllowNull(true)]
        string ForSite { get; set; }
        
        DateTime PublishOn { get; set; } 
    }
}
