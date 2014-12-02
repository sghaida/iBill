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
    public class DelegateRolesDataMapper : DataAccess<DelegateRole>
    {
        public bool IsUserDelegate(string userSipAccount)
        {
            DelegateRole role = null;
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("DelegeeSipAccount", userSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (!string.IsNullOrEmpty(role.ManagedUserSipAccount) && role.ManagedUser != null);
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
            condition.Add("DelegeeSipAccount", userSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (role.ManagedSiteID > 0 && role.ManagedSite != null);
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
            condition.Add("DelegeeSipAccount", userSipAccount);

            try
            {
                role = Get(whereConditions: condition, limit: 1).ToList<DelegateRole>().FirstOrDefault<DelegateRole>() ?? null;

                if (role != null)
                    return (role.ManagedSiteDepartmentID > 0 && role.ManagedSiteDepartment != null);
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
            conditions.Add("DelegeeSipAccount", userSipAccount);
            conditions.Add("DelegationType", DelegateTypeID);

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
