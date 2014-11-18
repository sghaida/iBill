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
        [DbColumnAttribute("ID")]
        public int ID { get; set; }

        [DbColumnAttribute("Announcement")]
        public string Announcement { get; set; }
        
        [IsAllowNull]
        [DbColumnAttribute("ForRole")]
        public int ForRole { get; set; }
        
        [IsAllowNull]
        [DbColumnAttribute("ForSite")]
        public int ForSite { get; set; }

        [DbColumnAttribute("PublishOn")]
        public DateTime PublishOn { get; set; }
    }
}
