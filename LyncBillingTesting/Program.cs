using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DAL;
using LyncBillingBase.Helpers;
using LyncBillingBase.Repository;


namespace LyncBillingTesting
{
    class Program
    {
        public static void Main(string[] args)
        {
            var _dbStorage = DataStorage.Instance;

            List<User> users = _dbStorage.Users.GetAll().ToList<User>();

            string x = string.Empty;
        }
    }
}
