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
        private static readonly Enums Enums = new Enums();
        private readonly AdLib _adRoutines = new AdLib();

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

            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime), phoneCall.SessionIdTime);

            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq), phoneCall.SessionIdSeq);

            if (DateTime.MinValue != phoneCall.ResponseTime)
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ResponseTime), phoneCall.ResponseTime);

            if (DateTime.MinValue != phoneCall.SessionEndTime)
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SessionEndTime), phoneCall.SessionEndTime);

            if (!string.IsNullOrEmpty(phoneCall.SourceUserUri))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SourceUserUri), phoneCall.SourceUserUri);

            if (!string.IsNullOrEmpty(phoneCall.SourceNumberUri))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri), phoneCall.SourceNumberUri);

            if (!string.IsNullOrEmpty(phoneCall.DestinationNumberUri))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri),
                    phoneCall.DestinationNumberUri);

            if (!string.IsNullOrEmpty(phoneCall.DestinationUserUri))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri),
                    phoneCall.DestinationUserUri);

            if (!string.IsNullOrEmpty(phoneCall.FromMediationServer))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.FromMediationServer),
                    phoneCall.FromMediationServer);

            if (!string.IsNullOrEmpty(phoneCall.ToMediationServer))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ToMediationServer), phoneCall.ToMediationServer);

            if (!string.IsNullOrEmpty(phoneCall.FromGateway))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.FromGateway), phoneCall.FromGateway);

            if (!string.IsNullOrEmpty(phoneCall.ToGateway))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ToGateway), phoneCall.ToGateway);

            if (!string.IsNullOrEmpty(phoneCall.SourceUserEdgeServer))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer),
                    phoneCall.SourceUserEdgeServer);

            if (!string.IsNullOrEmpty(phoneCall.DestinationUserEdgeServer))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer),
                    phoneCall.DestinationUserEdgeServer);

            if (!string.IsNullOrEmpty(phoneCall.ServerFqdn))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ServerFqdn), phoneCall.ServerFqdn);

            if (!string.IsNullOrEmpty(phoneCall.PoolFqdn))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.PoolFqdn), phoneCall.PoolFqdn);

            if (!string.IsNullOrEmpty(phoneCall.OnBehalf))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.OnBehalf), phoneCall.OnBehalf);

            if (!string.IsNullOrEmpty(phoneCall.ReferredBy))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ReferredBy), phoneCall.ReferredBy);

            if (!string.IsNullOrEmpty(phoneCall.CalleeUri))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.CalleeUri), phoneCall.CalleeUri);

            if (!string.IsNullOrEmpty(phoneCall.ChargingParty))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.ChargingParty), phoneCall.ChargingParty);

            if (!string.IsNullOrEmpty(phoneCall.MarkerCallToCountry))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.MarkerCallToCountry),
                    phoneCall.MarkerCallToCountry);

            if (!string.IsNullOrEmpty(phoneCall.MarkerCallType))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.MarkerCallType), phoneCall.MarkerCallType);

            if (!string.IsNullOrEmpty(phoneCall.UiCallType))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.UiCallType), phoneCall.UiCallType);


            if (!string.IsNullOrEmpty(phoneCall.UiUpdatedByUser))
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.UiUpdatedByUser), phoneCall.UiUpdatedByUser);

            if (DateTime.MinValue != phoneCall.UiMarkedOn)
                phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.UiMarkedOn), phoneCall.UiMarkedOn);

            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.MarkerCallTypeId), phoneCall.MarkerCallTypeId);
            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.MarkerCallCost), phoneCall.MarkerCallCost);
            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.MarkerCallFrom), phoneCall.MarkerCallFrom);
            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.MarkerCallTo), phoneCall.MarkerCallTo);
            phoneCallDict.Add(Enums.GetDescription(Enums.PhoneCalls.Duration), phoneCall.Duration);

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
                dataReader.GetDateTime(dataReader.GetOrdinal(Enums.GetDescription(Enums.PhoneCalls.SessionIdTime)));

            //phoneCall.SessionIdTime = Convert.ToDateTime(dataReader[enums.GetDescription(ENUMS.PhoneCalls.SessionIdTime)].ToString());
            phoneCall.SessionIdSeq =
                dataReader.GetInt32(dataReader.GetOrdinal(Enums.GetDescription(Enums.PhoneCalls.SessionIdSeq)));

            column = Enums.GetDescription(Enums.PhoneCalls.ResponseTime);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ResponseTime = dataReader.GetDateTime(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.SessionEndTime);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SessionEndTime = dataReader.GetDateTime(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceUserUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.SourceNumberUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceNumberUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationUserUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationNumberUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.FromMediationServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.FromMediationServer = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.ToMediationServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ToMediationServer = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.FromGateway);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.FromGateway = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.ToGateway);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ToGateway = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.SourceUserEdgeServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.SourceUserEdgeServer = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.DestinationUserEdgeServer);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.DestinationUserEdgeServer = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.ServerFqdn);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ServerFqdn = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.PoolFqdn);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.PoolFqdn = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.OnBehalf);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.OnBehalf = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.ReferredBy);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.ReferredBy = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.CalleeUri);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.CalleeUri = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.ChargingParty);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.ChargingParty = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.Duration);
            if (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty)
                phoneCall.Duration = dataReader.GetDecimal(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.MarkerCallFrom);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.MarkerCallFrom = dataReader.GetInt64(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.MarkerCallTo);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.MarkerCallTo = dataReader.GetInt64(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.MarkerCallToCountry);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.MarkerCallToCountry = dataReader.GetString(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.MarkerCallTypeId);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.MarkerCallTypeId = dataReader.GetInt32(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.MarkerCallCost);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.MarkerCallCost = dataReader.GetInt32(dataReader.GetOrdinal(column));

            column = Enums.GetDescription(Enums.PhoneCalls.MarkerCallType);
            if (ValidateColumnName(ref dataReader, ref column) &&
                (dataReader[column] != DBNull.Value || dataReader[column].ToString() != string.Empty))
                phoneCall.MarkerCallType = dataReader.GetString(dataReader.GetOrdinal(column));


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
                var number = Repo.NumberingPlan.Find(item => item.DialingPrefix == numberToParse);

                if (number != null)
                {
                    typeOfService = number.TypeOfService;
                    dialingPrefix = number.DialingPrefix;
                    return number.Iso3CountryCode;
                }
                numberToParse = numberToParse/10;
            }

            return "N/A";
        }

        public bool MatchDid(string phoneNumber, out string site, string sipAccount = null)
        {
            var tmpPhoneNumber = string.Empty;


            if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(sipAccount))
            {
                var userInfo = Repo.Users.Find(item => item.SipAccount.ToLower() == sipAccount.ToLower());

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

            foreach (var didEntry in Repo.Dids)
            {
                var did = didEntry.Regex;

                if (Regex.IsMatch(tmpPhoneNumber.Trim('+'), @"^" + did))
                {
                    var siteEntry = Repo.Sites.FirstOrDefault(item => item.Id == didEntry.SiteId);

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
                var userInfo = Repo.Users.Find(item => item.SipAccount.ToLower() == sipAccount.ToLower());
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

        public bool GetNgnDialingInfo(string phoneNumber, string sourceCountry, out long dialingPrefix,
            out string countryCode, out string callType)
        {
            dialingPrefix = 0;
            callType = "N/A";
            countryCode = "N/A";


            if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(sourceCountry))
            {
                var numberNgn = Repo.NumberingPlanNgn.Find(
                    item =>
                        item.Iso3CountryCode == Convert.ToString(ReturnEmptyIfNull(sourceCountry)) &&
                        Regex.IsMatch(phoneNumber, item.DialingCode)
                    );

                //Try to figure NGN call type
                if (numberNgn != null)
                {
                    callType = numberNgn.TypeOfService.Name;
                    countryCode = numberNgn.Iso3CountryCode;
                    long.TryParse(numberNgn.DialingCode.Trim('^').Trim('+'), out dialingPrefix);

                    return true;
                }
            }

            return false;
        }

        public PhoneCall UpdateChargingPartyField(PhoneCall phoneCall)
        {
            var destinationNumberLeadingChars = new List<char> {'+', '0'};

            if (phoneCall.CalleeUri == phoneCall.DestinationNumberUri || phoneCall.CalleeUri == null)
            {
                return phoneCall;
            }

            if (Regex.IsMatch(phoneCall.CalleeUri, @"\d{1,}@\w{1,}.*") ||
                Regex.IsMatch(phoneCall.CalleeUri, @"\d{1,};\w{1,}.*"))
            {
                //Try to fetch the calleeUri phone number
                //Two cases for matching two versions of the calleeUri
                var calleeUriCase1 = ReplaceStringWithPattern(phoneCall.CalleeUri, @"@\w{1,}.*",
                    charactersToBeTrimmed: destinationNumberLeadingChars);
                var calleeUriCase2 = ReplaceStringWithPattern(phoneCall.CalleeUri, @";\w{1,}.*",
                    charactersToBeTrimmed: destinationNumberLeadingChars);

                if (calleeUriCase1 == phoneCall.DestinationNumberUri.Trim('+') ||
                    calleeUriCase2 == phoneCall.DestinationNumberUri.Trim('+'))
                {
                    return phoneCall;
                }

                if (IsValidEmail(phoneCall.CalleeUri))
                {
                    phoneCall.ChargingParty = phoneCall.CalleeUri;
                    return phoneCall;
                }

                var newChargingParty = NormalizePhoneNumber(phoneCall.CalleeUri);

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

        public bool IsImEmail(string emailAddress)
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

        public object ReturnNullIfDbNull(object value)
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

                var userInfo = _adRoutines.GetUsersAttributesFromPhone(number);

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