using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "PhoneBook", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class PhoneBookContact : DataModel
    {
        [IsIdField]
        [DbColumn("ID")]
        public int Id { get; set; }

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