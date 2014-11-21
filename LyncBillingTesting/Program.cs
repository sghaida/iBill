using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DA;
using LyncBillingBase.DAL;
using LyncBillingBase.LookupTables;
using LyncBillingBase.Helpers;
using LyncBillingBase.Repository;



namespace LyncBillingTesting
{
    class Program
    {
        public  static string tolower(string text)
        {
            return text.ToLower();
        }

        public static void Main(string[] args)
        {
            var _dbStorage = DataStorage.Instance;

            Expression<Func<PhoneCall, bool>> expr = (item) => item.ChargingParty == "sghaida@ccc.gr" as string && item.SourceUserUri=="sghaida@ccc.gr";
            
            List<PhoneCall> phoneCalls = _dbStorage.PhoneCalls.Get(expr, "PhoneCalls2010").ToList();


          
        }
    }
}
