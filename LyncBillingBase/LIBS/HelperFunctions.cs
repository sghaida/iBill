using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace LyncBillingBase.LIBS
{
    public class HelperFunctions
    {
        public static bool GetResolvedConnecionIPAddress(string serverNameOrURL, out string resolvedIPAddress)
        {
            bool isResolved = false;
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
                            foreach (IPAddress var in hostEntry.AddressList)
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
            catch (Exception ex)
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
            var serializer = new XmlSerializer(typeof(T));

            using (var sw = new System.IO.StringWriter())
            using (var writer = new XmlTextWriter(sw))
            {
                serializer.Serialize(writer, source);
                return sw.ToString();
            }
        }

        public static T DeSerializeObject<T>(string xml)
        {
            using (System.IO.StringReader sr = new System.IO.StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        public static object ReturnZeroIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return 0;
            else if (value == null)
                return 0;
            else
                return value;
        }

        public static object ReturnEmptyIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return string.Empty;
            else if (value == null)
                return string.Empty;
            else
                return value;
        }

        public static object ReturnFalseIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return false;
            else if (value == null)
                return false;
            else
                return value;
        }

        public static object ReturnDateTimeMinIfNull(object value)
        {
            if (value == System.DBNull.Value)
                return DateTime.MinValue;
            else if (value == null)
                return DateTime.MinValue;
            else
                return value;
        }

        public static object ReturnNullIfDBNull(object value)
        {
            if (value == System.DBNull.Value)
                return '\0';
            else if (value == null)
                return '\0';
            else
                return value;
        }

        //This function formats the display-name of a user,
        //and removes unnecessary extra information.
        public static string FormatUserDisplayName(string displayName = null, string defaultValue = "tBill Users", bool returnNameIfExists = false, bool returnAddressPartIfExists = false)
        {
            //Get the first part of the Users's Display Name if s/he has a name like this: "firstname lastname (extra text)"
            //removes the "(extra text)" part
            if (!string.IsNullOrEmpty(displayName))
            {
                if (returnNameIfExists == true)
                    return Regex.Replace(displayName, @"\ \(\w{1,}\)", "");
                else
                    return (displayName.Split(' '))[0];
            }
            else
            {
                if (returnAddressPartIfExists == true)
                {
                    var emailParts = defaultValue.Split('@');
                    return emailParts[0];
                }
                else
                    return defaultValue;
            }
        }

        public static string FormatUserTelephoneNumber(string telephoneNumber)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(telephoneNumber))
            {
                //result = telephoneNumber.ToLower().Trim().Trim('+').Replace("tel:", "");
                result = telephoneNumber.ToLower().Trim().Replace("tel:", "");

                if (result.Contains(";"))
                {
                    if (!result.ToLower().Contains(";ext="))
                        result = result.Split(';')[0].ToString();
                }
            }

            return result;
        }

        public static bool IsValidEmail(string emailAddress)
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.IsMatch(emailAddress, pattern);
        }

    }
}
