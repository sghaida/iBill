using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class PhoneBook
    {
        public int ID { get; set; }
        public string SipAccount { get; set; }
        public string DestinationNumber { get; set; }
        public string DestinationCountry { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
