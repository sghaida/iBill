using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class NumberingPlansForNGNDataMapper : DataAccess<NumberingPlanForNGN>
    {
        public List<NumberingPlanForNGN> GetByPrefix(string DialingCode)
        {
            throw new NotImplementedException();
        }


        public List<NumberingPlanForNGN> GetByISO2CountryCode(string ISO2Code)
        {
            throw new NotImplementedException();
        }


        public List<NumberingPlanForNGN> GetByISO3CountryCode(string ISO3Code)
        {
            throw new NotImplementedException();
        }


        public string GetISO3CountryCodeByNumber(string TelephoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}
