using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataMappers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.Helpers
{
    public static class DataMapperExtensions
    {
        private static SitesDepartmentsDataMapper _sitesDepartmentsDataMapper = SitesDepartmentsDataMapper.Instance;


        /// <summary>
        /// This extension function operates over any DelegateRole object.
        /// It fills the site-department with its nested relations (Site, Department).
        /// </summary>
        /// <param name="delegateRole">DelegateRole object</param>
        public static DelegateRole IncludeSiteDepartments(this DelegateRole delegateRole)
        {
            try
            {
                // 
                // Get the site-department object
                SiteDepartment siteDepartment = _sitesDepartmentsDataMapper.GetById(delegateRole.ManagedSiteDepartmentID);

                //
                // Fill the site department object
                if (siteDepartment != null)
                {
                    delegateRole.ManagedSiteDepartment = siteDepartment;
                }
                else
                {
                    delegateRole.ManagedSiteDepartment = null;
                }

                return delegateRole;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }//end-function


        /// <summary>
        /// This extension function operates over any enumerable collection of DelegateRole objects.
        /// It fills the site-departments with their nested relations.
        /// </summary>
        /// <param name="delegateRoles">An enumerable collection of DelegateRole objects.</param>
        public static IEnumerable<DelegateRole> IncludeSiteDepartments(this IEnumerable<DelegateRole> delegateRoles)
        {
            IEnumerable<DelegateRole> sitesDelegates = new List<DelegateRole>();
            IEnumerable<DelegateRole> sitesDepartmentsDelegates = new List<DelegateRole>();
            IEnumerable<DelegateRole> userDelegates = new List<DelegateRole>();

            try
            {
                //
                // Get all sites departments
                IEnumerable<SiteDepartment> allSitesDepartments = _sitesDepartmentsDataMapper.GetAll();

                // Enable parallelization on the enumerable collections
                allSitesDepartments = allSitesDepartments.AsParallel<SiteDepartment>();
                delegateRoles = delegateRoles.AsParallel<DelegateRole>();

                //Fitler, join, and project
                sitesDelegates = delegateRoles.Where(item => item.ManagedSiteID > 0).ToList();
                sitesDepartmentsDelegates = delegateRoles.Where(item => item.ManagedSiteDepartmentID > 0).ToList();
                userDelegates = delegateRoles.Where(item => false == string.IsNullOrEmpty(item.ManagedUserSipAccount)).ToList();

                sitesDepartmentsDelegates =
                    (from role in sitesDepartmentsDelegates
                     join siteDepartment in allSitesDepartments on role.ManagedSiteDepartmentID equals siteDepartment.ID
                     select new DelegateRole
                     {
                         ID = role.ID,
                         DelegeeSipAccount = role.DelegeeSipAccount,
                         DelegationType = role.DelegationType,
                         ManagedUserSipAccount = role.ManagedUserSipAccount,
                         ManagedSiteID = role.ManagedSiteID,
                         ManagedSiteDepartmentID = siteDepartment.ID,
                         Description = role.Description,
                         //RELATIONS
                         ManagedUser = role.ManagedUser,
                         ManagedSiteDepartment = siteDepartment,
                         ManagedSite = role.ManagedSite
                     })
                     .AsEnumerable<DelegateRole>();

                //
                // Concatenate all the lists and return them
                return (userDelegates.Concat(sitesDelegates.Concat(sitesDepartmentsDelegates)).AsEnumerable<DelegateRole>());
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }//end-function


        /// <summary>
        /// This extension function operates over any enumerable collection of DepartmentHead objects.
        /// It fills the site-departments with their nested relations.
        /// </summary>
        /// <param name="delegateRoles">An enumerable collection of DepartmentHead objects.</param>
        public static DepartmentHeadRole IncludeSiteDepartments(this DepartmentHeadRole departmentHeadRole)
        {
            try
            {
                // 
                // Get the site-department object
                SiteDepartment siteDepartment = _sitesDepartmentsDataMapper.GetById(departmentHeadRole.SiteDepartmentID);

                //
                // Fill the site department object
                if (siteDepartment != null)
                {
                    departmentHeadRole.SiteDepartment = siteDepartment;
                }
                else
                {
                    departmentHeadRole.SiteDepartment = null;
                }

                return departmentHeadRole;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }//end-function


        /// <summary>
        /// This extension function operates over any enumerable collection of DepartmentHead objects.
        /// It fills the site-departments with their nested relations.
        /// </summary>
        /// <param name="delegateRoles">An enumerable collection of DepartmentHead objects.</param>
        public static IEnumerable<DepartmentHeadRole> IncludeSiteDepartments(this IEnumerable<DepartmentHeadRole> departmentHeadsRoles)
        {
            try
            {
                //
                // Get all sites departments
                IEnumerable<SiteDepartment> allSitesDepartments = _sitesDepartmentsDataMapper.GetAll();

                // Enable parallelization of the enumerable collections
                allSitesDepartments = allSitesDepartments.AsParallel<SiteDepartment>();
                departmentHeadsRoles = departmentHeadsRoles.AsParallel<DepartmentHeadRole>();

                //Fitler, join, and project
                departmentHeadsRoles =
                    (from role in departmentHeadsRoles
                     where (role.SiteDepartmentID > 0)
                     join siteDepartment in allSitesDepartments on role.SiteDepartmentID equals siteDepartment.ID
                     select new DepartmentHeadRole
                     {
                         ID = role.ID,
                         SipAccount = role.SipAccount,
                         SiteDepartmentID = role.SiteDepartmentID,
                         //RELATIONS
                         User = role.User,
                         SiteDepartment = siteDepartment
                     })
                     .AsEnumerable<DepartmentHeadRole>();

                return departmentHeadsRoles;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }//end-function

    }

}
