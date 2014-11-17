using LyncBillingBase.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DAL
{
    public class Departments
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }

        private static DBLib DBRoutines = new DBLib();

        public static List<Departments> GetDepartments(List<string> columns = null, Dictionary<string, object> whereConditions = null, int limit = 0)
        {
            
            List<Departments> departmentsList = new List<Departments>();

            return departmentsList;
        }

    }
}
