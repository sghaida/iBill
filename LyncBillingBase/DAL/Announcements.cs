using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{

    [TableName("Announcements")]
    public class Announcements
    {
        [IsIdField]
        public int ID { get; set; }
        
        public string Announcement { get; set; }
        
        [IsAllowNull]
        public int ForRole { get; set; }
        
        [IsAllowNull]
        public int ForSite { get; set; }
        
        public DateTime PublishOn { get; set; }
    }
}
