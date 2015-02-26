using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;
using System.Collections;
using System.Collections.Generic;

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


        public PhoneBookContact() { }

        public PhoneBookContact(int Id, string SipAccount, string Type, string Name, string DestinationNumber, string DestinationCountry)
        {
            this.Id = Id;
            this.SipAccount = SipAccount;
            this.Type = Type;
            this.Name = Name;
            this.DestinationNumber = DestinationNumber;
            this.DestinationCountry = DestinationCountry;
        }
    }


    public class PhoneBookContactComparer : IEqualityComparer<PhoneBookContact>
    {
        public bool Equals(PhoneBookContact firstContact, PhoneBookContact secondContact)
        {
            try
            {
                return (firstContact.DestinationNumber == secondContact.DestinationNumber);
            }
            catch(System.Exception)
            {
                return false;
            }
        }

        public int GetHashCode(PhoneBookContact contact)
        {
            return contact.DestinationNumber.GetHashCode();
        }
    }
}