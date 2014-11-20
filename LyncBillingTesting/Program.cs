using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DAL;
using LyncBillingBase.Helpers;
using LyncBillingBase.Repository;
using LyncBillingBase.DAL.Functions;


namespace LyncBillingTesting
{
    class Program
    {
        public static void Main(string[] args)
        {
            var _dbStorage = DataStorage.Instance;

            List<ChargeableCallsPerUser> data = _dbStorage.ChargeableCallsPerUSer.GetAll().ToList();

        }
    }
}
