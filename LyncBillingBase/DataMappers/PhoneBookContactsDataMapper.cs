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
        private static NumberingPlansDataMapper _numberingPlanDataMapper = new NumberingPlansDataMapper();

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

                Parallel.ForEach(userPhoneBookContacts, (contact) =>
                {
                    contact.TypeOfService = _numberingPlanDataMapper.GetTypeOfServiceByNumber(contact.DestinationNumber);
                });

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
                                        contact.TypeOfService = _numberingPlanDataMapper.GetTypeOfServiceByNumber(contact.DestinationNumber);

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
                    if (!string.IsNullOrEmpty(phoneBookEntry.DestinationNumber))
                    {
                        phoneBookEntry.DestinationNumber = HelperFunctions.FormatUserTelephoneNumber(phoneBookEntry.DestinationNumber);

                        if (string.IsNullOrEmpty(phoneBookEntry.DestinationCountry) || phoneBookEntry.DestinationCountry == "N/A")
                        {
                            phoneBookEntry.DestinationCountry = _numberingPlanDataMapper.GetIso3CountryCodeByNumber(phoneBookEntry.DestinationNumber);
                        }

                        //Either update or insert to the database
                        if (existingContacts.ContainsKey(phoneBookEntry.DestinationNumber))
                            this.Update(phoneBookEntry);
                        else
                            this.Insert(phoneBookEntry);
                    }
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
                    if(!string.IsNullOrEmpty(contact.DestinationNumber))
                    {
                        contact.DestinationNumber = HelperFunctions.FormatUserTelephoneNumber(contact.DestinationNumber);

                        if(string.IsNullOrEmpty(contact.DestinationCountry) || contact.DestinationCountry == "N/A")
                        {
                            contact.DestinationCountry = _numberingPlanDataMapper.GetIso3CountryCodeByNumber(contact.DestinationNumber);
                        }
                    
                        base.Update(contact);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneBookContacts"></param>
        public void InsertMany(List<PhoneBookContact> phoneBookContacts)
        {
            if (phoneBookContacts != null && phoneBookContacts.Any())
            {
                foreach (var contact in phoneBookContacts)
                {
                    if (!string.IsNullOrEmpty(contact.DestinationNumber))
                    {
                        contact.DestinationNumber = HelperFunctions.FormatUserTelephoneNumber(contact.DestinationNumber);

                        if (string.IsNullOrEmpty(contact.DestinationCountry) || contact.DestinationCountry == "N/A")
                        {
                            contact.DestinationCountry = _numberingPlanDataMapper.GetIso3CountryCodeByNumber(contact.DestinationNumber);
                        }

                        base.Insert(contact);
                    }
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool FixBrokenContacts()
        {
            bool status = false;

            try
            {
                var brokenPhoneBookContacts = (base.GetAll() ?? (new List<PhoneBookContact>()))
                    //.Where(item => 
                    //    string.IsNullOrEmpty(item.DestinationNumber) ||
                    //    string.IsNullOrEmpty(item.DestinationCountry))
                    .ToList<PhoneBookContact>();

                if(brokenPhoneBookContacts.Any())
                {
                    foreach(var contact in brokenPhoneBookContacts)
                    {
                        //
                        // First check the destination number
                        if(string.IsNullOrEmpty(contact.DestinationNumber))
                        {
                            // Delete the contact object
                            this.Delete(contact);
                            
                            // SKIP THE REST OF THE LOOP!
                            continue;
                        }

                        if(string.IsNullOrEmpty(contact.DestinationCountry))
                        {
                            // Determine the destination country based on the destination number
                            contact.DestinationNumber = HelperFunctions.FormatUserTelephoneNumber(contact.DestinationNumber);
                            contact.DestinationCountry = _numberingPlanDataMapper.GetIso3CountryCodeByNumber(contact.DestinationNumber) ?? "N/A";
                            
                            // Update the contact object
                            this.Update(contact);
                        }
                    }

                    status = true;
                }
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }

            return status;
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