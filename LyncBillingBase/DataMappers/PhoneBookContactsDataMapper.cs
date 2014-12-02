using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PhoneBookContactsDataMapper : DataAccess<PhoneBookContact>
    {
        public List<PhoneBookContact> GetDestinationNumbers(string UserSipAccount)
        {
            throw new NotImplementedException();
        }


        public static Dictionary<string, PhoneBookContact> GetAddressBook(string UserSipAccount)
        {
            throw new NotImplementedException();
        }
    }


    //This is used with LINQ Distinct method to compare if two contacts are the same before adding them to the "List of Contacts from Calls History"
    class PhoneBookContactComparer : IEqualityComparer<PhoneBookContact>
    {
        public bool Equals(PhoneBookContact firstContact, PhoneBookContact secondContact)
        {
            try
            {
                return (firstContact.DestinationNumber == secondContact.DestinationNumber && firstContact.DestinationCountry == secondContact.DestinationCountry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int GetHashCode(PhoneBookContact contact)
        {
            try
            {
                return (contact.DestinationNumber.ToString() + contact.DestinationCountry.ToString()).GetHashCode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
