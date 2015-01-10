using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DALDotNet.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PhoneBookContactsDataMapper : DataAccess<PhoneBookContact>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserSipAccount"></param>
        /// <returns></returns>
        public List<PhoneBookContact> GetBySipAccount(string UserSipAccount)
        {
            List<PhoneBookContact> userPhoneBookContacts = null;

            var linqDistinctComparer = new PhoneBookContactComparer();

            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", UserSipAccount);

            try
            {
                userPhoneBookContacts = Get(whereConditions: condition, limit: 0).ToList<PhoneBookContact>();

                if(userPhoneBookContacts != null && userPhoneBookContacts.Count > 0)
                {
                    userPhoneBookContacts = userPhoneBookContacts.Distinct(linqDistinctComparer).ToList<PhoneBookContact>();
                }

                return userPhoneBookContacts;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserSipAccount"></param>
        /// <returns></returns>
        public Dictionary<string, PhoneBookContact> GetAddressBook(string UserSipAccount)
        {
            List<PhoneBookContact> userPhoneBookContacts = null;
            Dictionary<string, PhoneBookContact> userAddressBook = null;

            try
            {
                userPhoneBookContacts = this.GetBySipAccount(UserSipAccount);

                if(userPhoneBookContacts != null && userPhoneBookContacts.Count > 0)
                {
                    //Initialize the userAddressBook
                    userAddressBook = new Dictionary<string, PhoneBookContact>();

                    //Fill the address book dictionary
                    Parallel.ForEach(userPhoneBookContacts,
                        (contact) =>
                        {
                            lock (userAddressBook)
                            {
                                if(false == string.IsNullOrEmpty(contact.DestinationNumber))
                                {
                                    if (false == userAddressBook.Keys.Contains(contact.DestinationNumber))
                                    {
                                        userAddressBook.Add(contact.DestinationNumber, contact);
                                    }
                                }
                            }
                        });
                }

                return userAddressBook;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }//end-of-data-mapper-class



    // LINQ Comparer
    // This is used with LINQ Distinct method to compare if two contacts are the same before adding them to the "List of Contacts from Calls History"
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
    }//end-of-comparer-class

}
