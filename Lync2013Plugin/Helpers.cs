using Lync2013Plugin.Implementation;
using LyncBillingBase.DataMappers;
using LyncBillingBase.DataModels;
using LyncBillingBase.Libs;
using LyncBillingBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lync2013Plugin
{
    public class Helpers
    {

        private ADLib adRoutines = new ADLib();

        public string FixNumberType(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "N/A";

            if (number.Contains(";"))
            {
                number = number.Split(';')[0].ToString();
            }

            number = number.Trim('+');


            return number;
        }

        public string GetCountryAndTypeOfServiceFromNumber(string phoneNumber, out long dialingPrefix, out string typeOfService)
        {
            long numberToParse = 0;
            dialingPrefix = 0;
            typeOfService = "N/A";

            long.TryParse(phoneNumber, out numberToParse);

            while (numberToParse > 0)
            {
                var number = Repo.numberingPlan.Find(item => item.DialingPrefix == numberToParse);

                if (number != null)
                {
                    typeOfService = number.TypeOfService;
                    dialingPrefix = number.DialingPrefix;
                    return number.ISO3CountryCode;
                }
                else
                {
                    numberToParse = numberToParse / 10;
                    continue;
                }
            }

            return "N/A";
        }

        public bool MatchDID(string phoneNumber, out string site, string sipAccount = null)
        {

            string tmpPhoneNumber = string.Empty;


            if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(sipAccount))
            {

                var userInfo = Repo.users.Find(item => item.SipAccount.ToLower() == sipAccount.ToLower());

                tmpPhoneNumber = userInfo != null ? FixNumberType(userInfo.TelephoneNumber) : @"N/A";

            }
            else if (string.IsNullOrEmpty(phoneNumber) && string.IsNullOrEmpty(sipAccount))
            {
                site = string.Empty;
                return false;
            }
            else
            {

                tmpPhoneNumber = phoneNumber;
            }

            foreach (DID didEntry in Repo.dids)
            {
                string did = didEntry.Regex;

                if (Regex.IsMatch(tmpPhoneNumber.Trim('+'), @"^" + did))
                {
                    var siteEntry = Repo.sites.FirstOrDefault(item => item.ID == didEntry.SiteID);

                    if (siteEntry != null)
                    {

                        site = siteEntry.Name;
                        return true;
                    }

                    site = didEntry.Description;
                    return true;
                }
                else
                {
                    continue;
                }
            }

            site = string.Empty;
            return false;
        }

        public long GetDialingPrefixInfo(string phoneNumber, out string callType, out string countryCode, string gatewayName = null, string sipAccount = null, string did = null)
        {
            long dialingPrefix = 0;

            callType = "N/A";
            countryCode = "N/A";

            string tmpPhoneNumber = string.Empty;

            if (phoneNumber == @"N/A" && !string.IsNullOrEmpty(sipAccount))
            {
                var userInfo = Repo.users.Find(item => item.SipAccount.ToLower() == sipAccount.ToLower());
                tmpPhoneNumber = userInfo != null ? FixNumberType(userInfo.TelephoneNumber) : @"N/A";
            }
            else
            {
                tmpPhoneNumber = phoneNumber;
            }

            //Get Country Code
            countryCode = GetCountryAndTypeOfServiceFromNumber(phoneNumber, out dialingPrefix, out callType);

            //call type and country code were already initialized, return dialing prefix
            return dialingPrefix;
        }

        public bool GetNGNDialingInfo(string phoneNumber, string sourceCountry, out long dialingPrefix, out string countryCode, out string callType)
        {
            dialingPrefix = 0;
            callType = "N/A";
            countryCode = "N/A";


            if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(sourceCountry))
            {
                var numberNGN = Repo.numberingPlanNGN.Find(
                    item =>
                        item.ISO3CountryCode == Convert.ToString(ReturnEmptyIfNull(sourceCountry)) &&
                        Regex.IsMatch(phoneNumber, item.DialingCode)
                );

                //Try to figure NGN call type
                if (numberNGN != null)
                {
                    callType = numberNGN.TypeOfService.Name;
                    countryCode = numberNGN.ISO3CountryCode;
                    long.TryParse(numberNGN.DialingCode.Trim('^').Trim('+'), out dialingPrefix);

                    return true;
                }
            }

            return false;
        }

        public PhoneCall UpdateChargingPartyField(PhoneCall phoneCall)
        {
            List<char> destinationNumberLeadingChars = new List<char>() { '+', '0' };

            if (phoneCall.CalleeURI == phoneCall.DestinationNumberUri || phoneCall.CalleeURI == null)
            {
                return phoneCall;
            }

            else if (Regex.IsMatch(phoneCall.CalleeURI, @"\d{1,}@\w{1,}.*") || Regex.IsMatch(phoneCall.CalleeURI, @"\d{1,};\w{1,}.*"))
            {
                //Try to fetch the calleeUri phone number
                //Two cases for matching two versions of the calleeUri
                string calleeUriCase1 = ReplaceStringWithPattern(phoneCall.CalleeURI, @"@\w{1,}.*", charactersToBeTrimmed: destinationNumberLeadingChars);
                string calleeUriCase2 = ReplaceStringWithPattern(phoneCall.CalleeURI, @";\w{1,}.*", charactersToBeTrimmed: destinationNumberLeadingChars);

                if (calleeUriCase1 == phoneCall.DestinationNumberUri.Trim('+') || calleeUriCase2 == phoneCall.DestinationNumberUri.Trim('+'))
                {
                    return phoneCall;
                }

                else if (IsValidEmail(phoneCall.CalleeURI))
                {
                    phoneCall.ChargingParty = phoneCall.CalleeURI;
                    return phoneCall;
                }

                else
                {
                    string newChargingParty = NormalizePhoneNumber(phoneCall.CalleeURI);

                    if (IsValidEmail(newChargingParty))
                        phoneCall.ChargingParty = newChargingParty;

                    return phoneCall;
                }
            }

            return phoneCall;
        }

        public string ReplaceStringWithPattern(string sourceString, string regexPattern, string replaceWith = "", List<char> charactersToBeTrimmed = null)
        {
            string result = string.Empty;

            result = Regex.Replace(sourceString, regexPattern, replaceWith);

            if (charactersToBeTrimmed != null)
            {
                foreach (char character in charactersToBeTrimmed)
                {
                    result = result.Trim().TrimStart(character);
                }
            }

            return result;
        }

        public bool IsValidEmail(string emailAddress)
        {
            emailAddress = emailAddress.ToLower();

            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.IsMatch(emailAddress, pattern);
        }

        public bool IsIMEmail(string emailAddress)
        {
            if (emailAddress.EndsWith("hotmail.com") ||
                emailAddress.EndsWith("yahoo.com") ||
                emailAddress.EndsWith("gmail.com") ||
                emailAddress.EndsWith("google.com"))
            {
                return true;
            }
            else { return false; }
        }

        public object ReturnZeroIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return 0;
            else
                return value;
        }

        public object ReturnEmptyIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return string.Empty;
            else if (value == null)
                return string.Empty;
            else
                return value;
        }

        public object ReturnNullIfDBNull(object value)
        {
            if (value == System.DBNull.Value)
                return '\0';
            else if (value == null)
                return '\0';
            else
                return value;
        }

        public object ReturnFalseIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return false;
            else
                return value;
        }

        public object ReturnDateTimeMinIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return DateTime.MinValue;
            else
                return value;
        }

        public string NormalizePhoneNumber(string phoneNumber)
        {
            string number = string.Empty;

            if (phoneNumber.StartsWith("+"))
            {
                number = Regex.Replace(phoneNumber, @"@\w{1,}.*", "");

                var userInfo = adRoutines.getUsersAttributesFromPhone(number);

                number = (userInfo != null && userInfo.SipAccount != null) ? userInfo.SipAccount.Replace("sip:", "") : number;

            }
            else
                number = phoneNumber;

            return number;
        }


    }


}
