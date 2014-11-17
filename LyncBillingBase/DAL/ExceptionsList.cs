 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class ExceptionsList
    {
        public long ID { set; get; }
        public string Entity { set; get; }
        public char EntityType { set; get; }
        public char ZeroCost { set; get; }
        public char AutoMark { get; set; }
        public int SiteID { set; get; }
        public string Description { set; get; }
    }
}
