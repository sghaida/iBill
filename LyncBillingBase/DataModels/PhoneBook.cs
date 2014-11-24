using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "PhoneBook", SourceType = Enums.DataSourceType.DBTable)]
    public class PhoneBookContact
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("SipAccount")]
        public string SipAccount { get; set; }

        [DbColumn("Type")]
        public string Type { get; set; }

        [DbColumn("Name")]
        public string Name { get; set; }

        [DbColumn("DestinationNumber")]
        public string DestinationNumber { get; set; }

        [DbColumn("DestinationCountry")]
        public string DestinationCountry { get; set; }
    }
}
