using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace CCC.UTILS.Helpers
{
    public static class HelperFunctions
    {
        public static bool GetResolvedConnecionIPAddress(string serverNameOrURL, out string resolvedIPAddress)
        {
            var isResolved = false;
            IPHostEntry hostEntry = null;
            IPAddress resolvIP = null;
            try
            {
                if (!IPAddress.TryParse(serverNameOrURL, out resolvIP))
                {
                    hostEntry = Dns.GetHostEntry(serverNameOrURL);

                    if (hostEntry != null && hostEntry.AddressList != null
                        && hostEntry.AddressList.Length > 0)
                    {
                        if (hostEntry.AddressList.Length == 1)
                        {
                            resolvIP = hostEntry.AddressList[0];
                            isResolved = true;
                        }
                        else
                        {
                            foreach (var var in hostEntry.AddressList)
                            {
                                if (var.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    resolvIP = var;
                                    isResolved = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    isResolved = true;
                }
            }
            catch (Exception)
            {
                isResolved = false;
                resolvIP = null;
            }
            finally
            {
                resolvedIPAddress = resolvIP.ToString();
            }

            return isResolved;
        }

        public static string SerializeObject<T>(T source)
        {
            var serializer = new XmlSerializer(typeof (T));

            using (var sw = new StringWriter())
            using (var writer = new XmlTextWriter(sw))
            {
                serializer.Serialize(writer, source);
                return sw.ToString();
            }
        }

        public static T DeSerializeObject<T>(string xml)
        {
            using (var sr = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof (T));
                return (T) serializer.Deserialize(sr);
            }
        }

        public static object ReturnZeroIfNull(this object value)
        {
            if (value == DBNull.Value)
                return 0;
            if (value == null)
                return 0;
            return value;
        }

        public static object ReturnEmptyIfNull(this object value)
        {
            if (value == DBNull.Value)
                return string.Empty;
            if (value == null)
                return string.Empty;
            return value;
        }

        public static object ReturnFalseIfNull(this object value)
        {
            if (value == DBNull.Value)
                return false;
            if (value == null)
                return false;
            return value;
        }

        public static object ReturnDateTimeMinIfNull(this object value)
        {
            if (value == DBNull.Value)
                return DateTime.MinValue;
            if (value == null)
                return DateTime.MinValue;
            return value;
        }

        public static object ReturnNullIfDBNull(this object value)
        {
            if (value == DBNull.Value)
                return '\0';
            if (value == null)
                return '\0';
            return value;
        }

        //This function formats the display-name of a user,
        //and removes unnecessary extra information.
        public static string FormatUserDisplayName(string displayName = null, string defaultValue = "tBill Users",
            bool returnNameIfExists = false, bool returnAddressPartIfExists = false)
        {
            //Get the first part of the Users's Display Name if s/he has a name like this: "firstname lastname (extra text)"
            //removes the "(extra text)" part
            if (!string.IsNullOrEmpty(displayName))
            {
                if (returnNameIfExists)
                    return Regex.Replace(displayName, @"\ \(\w{1,}\)", "");
                return (displayName.Split(' '))[0];
            }
            if (returnAddressPartIfExists)
            {
                var emailParts = defaultValue.Split('@');
                return emailParts[0];
            }
            return defaultValue;
        }

        public static string FormatUserTelephoneNumber(this string telephoneNumber)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(telephoneNumber))
            {
                //result = telephoneNumber.ToLower().Trim().Trim('+').Replace("tel:", "");
                result = telephoneNumber.ToLower().Trim().Replace("tel:", "");

                if (result.Contains(";"))
                {
                    if (!result.ToLower().Contains(";ext="))
                        result = result.Split(';')[0];
                }
            }

            return result;
        }

        public static bool IsValidEmail(this string emailAddress)
        {
            var pattern =
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.IsMatch(emailAddress, pattern);
        }

        public static string ConvertDate(this DateTime datetTime, bool excludeHoursAndMinutes = false)
        {
            if (datetTime != DateTime.MinValue || datetTime != null)
            {
                if (excludeHoursAndMinutes)
                    return datetTime.ToString("yyyy-MM-dd");
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            return null;
        }

        public static string ConvertSecondsToReadable(this int secondsParam)
        {
            var hours = Convert.ToInt32(Math.Floor((double) (secondsParam/3600)));
            var minutes = Convert.ToInt32(Math.Floor((double) (secondsParam - (hours*3600))/60));
            var seconds = secondsParam - (hours*3600) - (minutes*60);

            var hours_str = hours.ToString();
            var mins_str = minutes.ToString();
            var secs_str = seconds.ToString();

            if (hours < 10)
            {
                hours_str = "0" + hours_str;
            }

            if (minutes < 10)
            {
                mins_str = "0" + mins_str;
            }
            if (seconds < 10)
            {
                secs_str = "0" + secs_str;
            }

            return hours_str + ':' + mins_str + ':' + secs_str;
        }

        public static string ConvertSecondsToReadable(this long secondsParam)
        {
            var hours = Convert.ToInt32(Math.Floor((double) (secondsParam/3600)));
            var minutes = Convert.ToInt32(Math.Floor((double) (secondsParam - (hours*3600))/60));
            var seconds = Convert.ToInt32(secondsParam - (hours*3600) - (minutes*60));

            var hours_str = hours.ToString();
            var mins_str = minutes.ToString();
            var secs_str = seconds.ToString();

            if (hours < 10)
            {
                hours_str = "0" + hours_str;
            }

            if (minutes < 10)
            {
                mins_str = "0" + mins_str;
            }
            if (seconds < 10)
            {
                secs_str = "0" + secs_str;
            }

            return hours_str + ':' + mins_str + ':' + secs_str;
        }

        /// <summary>
        ///     Gets the Name of DB table Field
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string Description(this Enum enumObject)
        {
            var fieldInfo = enumObject.GetType().GetField(enumObject.ToString());

            var descAttributes =
                (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false);

            if (descAttributes != null && descAttributes.Length > 0)
            {
                return descAttributes[0].Description;
            }
            return enumObject.ToString();
        }

        /// <summary>
        ///     Gets the DefaultValue attribute of the enum
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string Value(this Enum enumObject)
        {
            var fieldInfo = enumObject.GetType().GetField(enumObject.ToString());

            var valueAttributes =
                (DefaultValueAttribute[]) fieldInfo.GetCustomAttributes(typeof (DefaultValueAttribute), false);

            if (valueAttributes != null && valueAttributes.Length > 0)
            {
                return valueAttributes[0].Value.ToString();
            }
            return enumObject.ToString();
        }
    }
}