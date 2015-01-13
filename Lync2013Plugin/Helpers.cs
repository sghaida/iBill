using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CCC.UTILS.Libs;
using Lync2013Plugin.Implementation;
using LyncBillingBase.DataModels;

namespace Lync2013Plugin
{
    public class Helpers
    {
        private static readonly ENUMS enums = new ENUMS();
        private readonly ADLib adRoutines = new ADLib();

        private static void ParallelWhile(Func<bool> condition, Action<ParallelLoopState> body)
        {
            Parallel.ForEach(Infinite(), (ignored, loopState) =>
            {
                if (condition()) body(loopState);
                else loopState.Stop();
            });
        }

        private static IEnumerable<bool> Infinite()
        {
            while (true) yield return true;
        }

        private static bool ValidateColumnName(ref OleDbDataReader dataReader, ref string columnName)
        {
            try
            {
                if (dataReader.GetOrdinal(columnName) >= 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /***
         * PUBLIC STATIC METHODS
         */

        #region Static-Public-Methods

        public static bool IsNull(object value)
        {
            if (value == null || value == DBNull.Value)
                return true;
            return false;
        }

        public static string ConvertDate(DateTime datetTime)
        {
            if (datetTime != DateTime.MinValue || datetTime != null)
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return null;
        }

        /***
        * This converts a PhoneCall object to a dictionary.
        */

        public static Dictionary<string, object> ConvertPhoneCallToDictionary(PhoneCall phoneCall)
        {
            var phoneCallDict = new Dictionary<string, object>();

            phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.SessionIdTime), phoneCall.SessionIdTime);

            phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.SessionIdSeq), phoneCall.SessionIdSeq);

            if (DateTime.MinValue != phoneCall.ResponseTime)
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.ResponseTime), phoneCall.ResponseTime);

            if (DateTime.MinValue != phoneCall.SessionEndTime)
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.SessionEndTime), phoneCall.SessionEndTime);

            if (!string.IsNullOrEmpty(phoneCall.SourceUserUri))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.SourceUserUri), phoneCall.SourceUserUri);

            if (!string.IsNullOrEmpty(phoneCall.SourceNumberUri))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.SourceNumberUri), phoneCall.SourceNumberUri);

            if (!string.IsNullOrEmpty(phoneCall.DestinationNumberUri))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.DestinationNumberUri),
                    phoneCall.DestinationNumberUri);

            if (!string.IsNullOrEmpty(phoneCall.DestinationUserUri))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.DestinationUserUri),
                    phoneCall.DestinationUserUri);

            if (!string.IsNullOrEmpty(phoneCall.FromMediationServer))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.FromMediationServer),
                    phoneCall.FromMediationServer);

            if (!string.IsNullOrEmpty(phoneCall.ToMediationServer))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.ToMediationServer), phoneCall.ToMediationServer);

            if (!string.IsNullOrEmpty(phoneCall.FromGateway))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.FromGateway), phoneCall.FromGateway);

            if (!string.IsNullOrEmpty(phoneCall.ToGateway))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.ToGateway), phoneCall.ToGateway);

            if (!string.IsNullOrEmpty(phoneCall.SourceUserEdgeServer))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.SourceUserEdgeServer),
                    phoneCall.SourceUserEdgeServer);

            if (!string.IsNullOrEmpty(phoneCall.DestinationUserEdgeServer))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.DestinationUserEdgeServer),
                    phoneCall.DestinationUserEdgeServer);

            if (!string.IsNullOrEmpty(phoneCall.ServerFQDN))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.ServerFQDN), phoneCall.ServerFQDN);

            if (!string.IsNullOrEmpty(phoneCall.PoolFQDN))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.PoolFQDN), phoneCall.PoolFQDN);

            if (!string.IsNullOrEmpty(phoneCall.OnBehalf))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.OnBehalf), phoneCall.OnBehalf);

            if (!string.IsNullOrEmpty(phoneCall.ReferredBy))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.ReferredBy), phoneCall.ReferredBy);

            if (!string.IsNullOrEmpty(phoneCall.CalleeURI))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.CalleeURI), phoneCall.CalleeURI);

            if (!string.IsNullOrEmpty(phoneCall.ChargingParty))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.ChargingParty), phoneCall.ChargingParty);

            if (!string.IsNullOrEmpty(phoneCall.Marker_CallToCountry))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.Marker_CallToCountry),
                    phoneCall.Marker_CallToCountry);

            if (!string.IsNullOrEmpty(phoneCall.Marker_CallType))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.Marker_CallType), phoneCall.Marker_CallType);

            if (!string.IsNullOrEmpty(phoneCall.UI_CallType))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.UI_CallType), phoneCall.UI_CallType);


            if (!string.IsNullOrEmpty(phoneCall.UI_UpdatedByUser))
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.UI_UpdatedByUser), phoneCall.UI_UpdatedByUser);

            if (DateTime.MinValue != phoneCall.UI_MarkedOn)
                phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.UI_MarkedOn), phoneCall.UI_MarkedOn);

            phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.Marker_CallTypeID), phoneCall.Marker_CallTypeID);
            phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.Marker_CallCost), phoneCall.Marker_CallCost);
            phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.Marker_CallFrom), phoneCall.Marker_CallFrom);
            phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.Marker_CallTo), phoneCall.Marker_CallTo);
            phoneCallDict.Add(enums.GetDescription(ENUMS.PhoneCalls.Duration), phoneCall.Duration);

            return phoneCallDict;
        }

        /***
         * This is used in the CallMarker classes, to fill the PhoneCall objects from the database reader.
         */

        public static PhoneCall FillPhoneCallFromOleDataReader(OleDbDataReader dataReader)
        {
            var column = string.Empty;
            var phoneCall = new PhoneCall();


            //Start filling the PhoneCall object

            phoneCall.SessionIdTime =
                dataReader.GetDateTime(dataReader.GetOrdinal(enums.GetDescription(ENUMS.PhoneCalls.SessionIdTime)));

            //phoneCall.SessionIdTime = Convert.ToDateTime(dataReader[enums.GetDescription(ENUMS.PhoneCalls.SessionIdTime)].ToString());
            phoneCall.SessionIdSeq =
                dataReader.GetInt32(dataReader.GetOrdinal(enums.GetDescription(ENUMS.PhoneCalls.SessionIdSeq)));

            column = enums.GetDescription(ENUMS.PhoneCalls.ResponseTime);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ResponseTime = dataReader.GetDateTime(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.SessionEndTime);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SessionEndTime = dataReader.GetDateTime(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.SourceUserUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceUserUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.SourceNumberUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceNumberUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.DestinationUserUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationUserUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.DestinationNumberUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationNumberUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.FromMediationServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.FromMediationServer = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.ToMediationServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ToMediationServer = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.FromGateway);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.FromGateway = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.ToGateway);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ToGateway = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.SourceUserEdgeServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceUserEdgeServer = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.DestinationUserEdgeServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationUserEdgeServer = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.ServerFQDN);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ServerFQDN = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.PoolFQDN);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.PoolFQDN = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.OnBehalf);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.OnBehalf = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.ReferredBy);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ReferredBy = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.CalleeURI);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.CalleeURI = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.ChargingParty);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.ChargingParty = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.Duration);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Duration = dataReader.GetDecimal(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.Marker_CallFrom);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallFrom = dataReader.GetInt64(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.Marker_CallTo);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallTo = dataReader.GetInt64(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.Marker_CallToCountry);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallToCountry = dataReader.GetString(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.Marker_CallTypeID);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallTypeID = dataReader.GetInt32(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.Marker_CallCost);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallCost = dataReader.GetInt32(dataReader.GetOrdinal(column));

            column = enums.GetDescription(ENUMS.PhoneCalls.Marker_CallType);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.Marker_CallType = dataReader.GetString(dataReader.GetOrdinal(column));


            //Return teh filled object
            return phoneCall;
        }

        public static void ResetTime(ref DateTime dateTime)
        {
            dateTime = dateTime.AddHours(-dateTime.Hour);
            dateTime = dateTime.AddMinutes(-dateTime.Minute);
            dateTime = dateTime.AddSeconds(-dateTime.Second);
            dateTime = dateTime.AddMilliseconds(-dateTime.Millisecond);
        }

        #endregion

        /***
         * PUBLIC NON-STATIC METHODS
         */

        #region Non-Static-Public-Methods

        public string FixNumberType(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "N/A";

            if (number.Contains(";"))
            {
                number = number.Split(';')[0];
            }

            number = number.Trim('+');


            return number;
        }

        public string GetCountryAndTypeOfServiceFromNumber(string phoneNumber, out long dialingPrefix,
            out string typeOfService)
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
                numberToParse = numberToParse/10;
            }

            return "N/A";
        }

        public bool MatchDID(string phoneNumber, out string site, string sipAccount = null)
        {
            var tmpPhoneNumber = string.Empty;


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

            foreach (var didEntry in Repo.dids)
            {
                var did = didEntry.Regex;

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
            }

            site = string.Empty;
            return false;
        }

        public long GetDialingPrefixInfo(string phoneNumber, out string callType, out string countryCode,
            string gatewayName = null, string sipAccount = null, string did = null)
        {
            long dialingPrefix = 0;

            callType = "N/A";
            countryCode = "N/A";

            var tmpPhoneNumber = string.Empty;

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

        public bool GetNGNDialingInfo(string phoneNumber, string sourceCountry, out long dialingPrefix,
            out string countryCode, out string callType)
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
            var destinationNumberLeadingChars = new List<char> {'+', '0'};

            if (phoneCall.CalleeURI == phoneCall.DestinationNumberUri || phoneCall.CalleeURI == null)
            {
                return phoneCall;
            }

            if (Regex.IsMatch(phoneCall.CalleeURI, @"\d{1,}@\w{1,}.*") ||
                Regex.IsMatch(phoneCall.CalleeURI, @"\d{1,};\w{1,}.*"))
            {
                //Try to fetch the calleeUri phone number
                //Two cases for matching two versions of the calleeUri
                var calleeUriCase1 = ReplaceStringWithPattern(phoneCall.CalleeURI, @"@\w{1,}.*",
                    charactersToBeTrimmed: destinationNumberLeadingChars);
                var calleeUriCase2 = ReplaceStringWithPattern(phoneCall.CalleeURI, @";\w{1,}.*",
                    charactersToBeTrimmed: destinationNumberLeadingChars);

                if (calleeUriCase1 == phoneCall.DestinationNumberUri.Trim('+') ||
                    calleeUriCase2 == phoneCall.DestinationNumberUri.Trim('+'))
                {
                    return phoneCall;
                }

                if (IsValidEmail(phoneCall.CalleeURI))
                {
                    phoneCall.ChargingParty = phoneCall.CalleeURI;
                    return phoneCall;
                }

                var newChargingParty = NormalizePhoneNumber(phoneCall.CalleeURI);

                if (IsValidEmail(newChargingParty))
                    phoneCall.ChargingParty = newChargingParty;

                return phoneCall;
            }

            return phoneCall;
        }

        public string ReplaceStringWithPattern(string sourceString, string regexPattern, string replaceWith = "",
            List<char> charactersToBeTrimmed = null)
        {
            var result = string.Empty;

            result = Regex.Replace(sourceString, regexPattern, replaceWith);

            if (charactersToBeTrimmed != null)
            {
                foreach (var character in charactersToBeTrimmed)
                {
                    result = result.Trim().TrimStart(character);
                }
            }

            return result;
        }

        public bool IsValidEmail(string emailAddress)
        {
            emailAddress = emailAddress.ToLower();

            var pattern =
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

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
            return false;
        }

        public object ReturnZeroIfNull(object value)
        {
            if (value == DBNull.Value)
                return 0;
            return value;
        }

        public object ReturnEmptyIfNull(object value)
        {
            if (value == DBNull.Value)
                return string.Empty;
            if (value == null)
                return string.Empty;
            return value;
        }

        public object ReturnNullIfDBNull(object value)
        {
            if (value == DBNull.Value)
                return '\0';
            if (value == null)
                return '\0';
            return value;
        }

        public object ReturnFalseIfNull(object value)
        {
            if (value == DBNull.Value)
                return false;
            return value;
        }

        public object ReturnDateTimeMinIfNull(object value)
        {
            if (value == DBNull.Value)
                return DateTime.MinValue;
            return value;
        }

        public string NormalizePhoneNumber(string phoneNumber)
        {
            var number = string.Empty;

            if (phoneNumber.StartsWith("+"))
            {
                number = Regex.Replace(phoneNumber, @"@\w{1,}.*", "");

                var userInfo = adRoutines.getUsersAttributesFromPhone(number);

                number = (userInfo != null && userInfo.SipAccount != null)
                    ? userInfo.SipAccount.Replace("sip:", "")
                    : number;
            }
            else
                number = phoneNumber;

            return number;
        }

        #endregion
    }
}