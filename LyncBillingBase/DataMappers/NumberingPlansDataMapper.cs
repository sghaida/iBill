using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class NumberingPlansDataMapper : DataAccess<NumberingPlan>
    {
        public List<NumberingPlan> GetByPrefix(Int64 DialingPrefix)
        {
            throw new NotImplementedException();
        }


        public List<NumberingPlan> GetByISO2CountryCode(string ISO2Code)
        {
            throw new NotImplementedException();
        }


        public List<NumberingPlan> GetByISO3CountryCode(string ISO3Code)
        {
            throw new NotImplementedException();
        }


        public string GetISO3CountryCodeByNumber(string TelephoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}
