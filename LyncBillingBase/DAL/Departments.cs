using LyncBillingBase.LIBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            DataTable dt;
            List<Departments> departmentsList = new List<Departments>();

            try
            {
                dt = DBRoutines.SELECT(Enums.GetDescription(Enums.Departments.TableName), columns, whereConditions, limit);

                departmentsList = dt.ToList<Departments>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return departmentsList;
        }

    }
}
