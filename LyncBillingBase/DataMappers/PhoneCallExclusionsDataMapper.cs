using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class PhoneCallExclusionsDataMapper : DataAccess<PhoneCallExclusion>
    {
        /***
         * Helper functions. These are very specific to the types of data in the data source.
         */
        private string LookUpExceptionType(char exceptionType)
        {
            switch(exceptionType)
            {
                case 'S': return "Source";
                case 'D': return "Destination";
                default: return "N/A";
            }
        }

        private string LookUpZeroCost(char zeroCost)
        {
            switch(zeroCost)
            {
                case 'Y': return "Yes";
                case 'N': return "No";
                default: return "N/A";
            }
        }

        private string LookUpAutoMark(char autoMarkType)
        {
            switch(autoMarkType)
            {
                case 'B': return "Business";
                case 'P': return "Personal";
                default: return "DISABLED";
            }
        }

        private string CleanTheExceptionBody(string phoneCallExclusionBodyText)
        {
            string cleanedupVersion = string.Empty;
            char[] unhealthyCharacters = new char['+'];

            if (!string.IsNullOrEmpty(phoneCallExclusionBodyText))
            {
                cleanedupVersion = phoneCallExclusionBodyText.Trim().Trim('+');

                if (cleanedupVersion.StartsWith("0")) //Also holds when it starts with "00" !!
                    cleanedupVersion = Convert.ToString(Convert.ToInt64(cleanedupVersion));
            }

            return cleanedupVersion;
        }


        public List<PhoneCallExclusion> GetBySiteID(int SiteID)
        {
            throw new NotImplementedException();
        }


        public List<PhoneCallExclusion> GetSourcesBySiteID(int SiteID)
        {
            throw new NotImplementedException();
        }


        public List<PhoneCallExclusion> GetDestinationsBySiteID(int SiteID)
        {
            throw new NotImplementedException();
        }
    }
}
