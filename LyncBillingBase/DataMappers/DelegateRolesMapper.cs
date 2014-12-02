using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class DelegateRolesMapper : DataAccess<DelegateRole>
    {
        public bool IsUserDelegate(string userSipAccount)
        {
            DelegateRole role = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("Delegee", userSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (role.DelegeeUser != null);
                else
                    return false;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public bool IsSiteDelegate(string userSipAccount)
        {
            DelegateRole role = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("Delegee", userSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (role.DelegeeSite != null);
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public bool IsDepartmentDelegate(string userSipAccount)
        {
            DelegateRole role = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("Delegee", userSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (role.DelegeeDepartment != null);
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public List<DelegateRole> GetDelegees(string userSipAccount, int DelegateTypeID)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("Delegee", userSipAccount);
            conditions.Add("DelegeeType", DelegateTypeID);

            try
            {
                return Get(whereConditions: conditions, limit: 1).ToList<DelegateRole>();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
