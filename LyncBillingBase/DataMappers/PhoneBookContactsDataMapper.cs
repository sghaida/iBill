using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CCC.UTILS.Helpers;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PhoneBookContactsDataMapper : DataAccess<PhoneBookContact>
    {
        /// <summary>
        /// </summary>
        /// <param name="userSipAccount"></param>
        /// <returns></returns>
        public List<PhoneBookContact> GetBySipAccount(string userSipAccount)
        {
            List<PhoneBookContact> userPhoneBookContacts = null;

            var linqDistinctComparer = new PhoneBookContactComparer();

            var condition = new Dictionary<string, object>();
            condition.Add("SipAccount", userSipAccount);

            try
            {
                userPhoneBookContacts = Get(condition, 0).ToList();

                if (userPhoneBookContacts != null && userPhoneBookContacts.Count > 0)
                {
                    userPhoneBookContacts = userPhoneBookContacts.Distinct(linqDistinctComparer).ToList();
                }

                return userPhoneBookContacts;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userSipAccount"></param>
        /// <returns></returns>
        public Dictionary<string, PhoneBookContact> GetAddressBook(string userSipAccount)
        {
            List<PhoneBookContact> userPhoneBookContacts = null;
            Dictionary<string, PhoneBookContact> userAddressBook = null;

            try
            {
                userPhoneBookContacts = GetBySipAccount(userSipAccount);

                if (userPhoneBookContacts != null && userPhoneBookContacts.Count > 0)
                {
                    //Initialize the userAddressBook
                    userAddressBook = new Dictionary<string, PhoneBookContact>();

                    //Fill the address book dictionary
                    Parallel.ForEach(userPhoneBookContacts,
                        contact =>
                        {
                            lock (userAddressBook)
                            {
                                if (false == string.IsNullOrEmpty(contact.DestinationNumber))
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
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sipAccount"></param>
        /// <param name="phoneBookEntries"></param>
        public void AddOrUpdatePhoneBookEntries(string sipAccount, List<PhoneBookContact> phoneBookEntries)
        {
            Dictionary<string, PhoneBookContact> existingContacts = this.GetAddressBook(sipAccount);

            foreach (PhoneBookContact phoneBookEntry in phoneBookEntries)
            {
                try
                {
                    phoneBookEntry.DestinationNumber = HelperFunctions.FormatUserTelephoneNumber(phoneBookEntry.DestinationNumber);

                    //Either update or insert to the database
                    if (existingContacts.ContainsKey(phoneBookEntry.DestinationNumber))
                        this.Update(phoneBookEntry);
                    else
                        this.Insert(phoneBookEntry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneBookContacts"></param>
        public void UpdateMany(List<PhoneBookContact> phoneBookContacts)
        {
            if (phoneBookContacts != null && phoneBookContacts.Count > 0)
            {
                foreach (var contact in phoneBookContacts)
                {
                    base.Update(contact);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneBookContacts"></param>
        public void InsertMany(List<PhoneBookContact> phoneBookContacts)
        {
            if (phoneBookContacts != null && phoneBookContacts.Count > 0)
            {
                foreach (var contact in phoneBookContacts)
                {
                    base.Insert(contact);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneBookContacts"></param>
        public void DeleteMany(List<PhoneBookContact> phoneBookContacts)
        {
            if (phoneBookContacts != null && phoneBookContacts.Count > 0)
            {
                foreach(var contact in phoneBookContacts)
                {
                    base.Delete(contact);
                }
            }
        }
    } //end-of-data-mapper-class


    // LINQ Comparer
    // This is used with LINQ Distinct method to compare if two contacts are the same before adding them to the "List of Contacts from Calls History"
    internal class PhoneBookContactComparer : IEqualityComparer<PhoneBookContact>
    {
        public bool Equals(PhoneBookContact firstContact, PhoneBookContact secondContact)
        {
            try
            {
                return (firstContact.DestinationNumber == secondContact.DestinationNumber &&
                        firstContact.DestinationCountry == secondContact.DestinationCountry);
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
                return (contact.DestinationNumber + contact.DestinationCountry).GetHashCode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    } //end-of-comparer-class
}