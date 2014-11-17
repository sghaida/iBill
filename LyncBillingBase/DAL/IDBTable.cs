using LyncBillingBase.LIBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LyncBillingBase.DAL
{
    abstract class DBTable<T>
    {
        private static DBLib DBRoutines = new DBLib();

        int Insert(T obj) 
        {
            int rowID = 0;
            Dictionary<string, object> columnsValues = new Dictionary<string, object>(); ;



            if (obj != null)
            {
                //Set Part
                if (!string.IsNullOrEmpty(obj.RoleName))
                    columnsValues.Add(Enums.GetDescription(Enums.Roles.RoleName), obj.RoleName);

                if (!string.IsNullOrEmpty(obj.RoleDescription))
                    columnsValues.Add(Enums.GetDescription(Enums.Roles.RoleDescription), obj.RoleDescription);

                //Execute Insert
                try
                {
                    rowID = DBRoutines.INSERT(Enums.GetDescription(Enums.Roles.TableName), columnsValues, Enums.GetDescription(Enums.Roles.RoleID));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return rowID;
        }
    }
}
