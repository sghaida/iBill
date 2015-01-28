using System;
using System.Collections.Generic;
using System.Linq;
using LyncBillingBase.DataMappers;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.Helpers
{
    public static class DataMapperExtensions
    {
        private static readonly SitesDepartmentsDataMapper SitesDepartmentsDataMapper =
            SitesDepartmentsDataMapper.Instance;

        /// <summary>
        ///     This extension function operates over any DelegateRole object.
        ///     It fills the site-department with its nested relations (Site, Department).
        /// </summary>
        /// <param name="delegateRole">DelegateRole object</param>
        public static DelegateRole IncludeSiteDepartments(this DelegateRole delegateRole)
        {
            try
            {
                // 
                // Get the site-department object
                var siteDepartment = SitesDepartmentsDataMapper.GetById(delegateRole.ManagedSiteDepartmentId);

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
        } //end-function

        /// <summary>
        ///     This extension function operates over any enumerable collection of DelegateRole objects.
        ///     It fills the site-departments with their nested relations.
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
                var allSitesDepartments = SitesDepartmentsDataMapper.GetAll();

                // Enable parallelization on the enumerable collections
                allSitesDepartments = allSitesDepartments.AsParallel();
                delegateRoles = delegateRoles.AsParallel();

                //Fitler, join, and project
                sitesDelegates = delegateRoles.Where(item => item.ManagedSiteId > 0).ToList();
                sitesDepartmentsDelegates = delegateRoles.Where(item => item.ManagedSiteDepartmentId > 0).ToList();
                userDelegates = delegateRoles.Where(item => false == string.IsNullOrEmpty(item.ManagedUserSipAccount)).ToList();

                sitesDepartmentsDelegates =
                    (from role in sitesDepartmentsDelegates
                        join siteDepartment in allSitesDepartments on role.ManagedSiteDepartmentId equals
                            siteDepartment.Id
                        select new DelegateRole
                        {
                            Id = role.Id,
                            DelegeeSipAccount = role.DelegeeSipAccount,
                            DelegationType = role.DelegationType,
                            ManagedUserSipAccount = role.ManagedUserSipAccount,
                            ManagedSiteId = role.ManagedSiteId,
                            ManagedSiteDepartmentId = siteDepartment.Id,
                            Description = role.Description,
                            //RELATIONS
                            DelegeeAccount = role.DelegeeAccount,
                            ManagedUser = role.ManagedUser,
                            ManagedSiteDepartment = siteDepartment,
                            ManagedSite = role.ManagedSite
                        })
                        .AsEnumerable<DelegateRole>();

                //
                // Concatenate all the lists and return them
                return (userDelegates.Concat(sitesDelegates.Concat(sitesDepartmentsDelegates)).AsEnumerable());
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        } //end-function

        /// <summary>
        ///     This extension function operates over any enumerable collection of DepartmentHead objects.
        ///     It fills the site-departments with their nested relations.
        /// </summary>
        /// <param name="delegateRoles">An enumerable collection of DepartmentHead objects.</param>
        public static DepartmentHeadRole IncludeSiteDepartments(this DepartmentHeadRole departmentHeadRole)
        {
            try
            {
                // 
                // Get the site-department object
                var siteDepartment = SitesDepartmentsDataMapper.GetById(departmentHeadRole.SiteDepartmentId);

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
        } //end-function

        /// <summary>
        ///     This extension function operates over any enumerable collection of DepartmentHead objects.
        ///     It fills the site-departments with their nested relations.
        /// </summary>
        /// <param name="delegateRoles">An enumerable collection of DepartmentHead objects.</param>
        public static IEnumerable<DepartmentHeadRole> IncludeSiteDepartments(
            this IEnumerable<DepartmentHeadRole> departmentHeadsRoles)
        {
            try
            {
                //
                // Get all sites departments
                var allSitesDepartments = SitesDepartmentsDataMapper.GetAll();

                // Enable parallelization of the enumerable collections
                allSitesDepartments = allSitesDepartments.AsParallel();
                departmentHeadsRoles = departmentHeadsRoles.AsParallel();

                //Fitler, join, and project
                departmentHeadsRoles =
                    (from role in departmentHeadsRoles
                        where (role.SiteDepartmentId > 0)
                        join siteDepartment in allSitesDepartments on role.SiteDepartmentId equals siteDepartment.Id
                        select new DepartmentHeadRole
                        {
                            Id = role.Id,
                            SipAccount = role.SipAccount,
                            SiteDepartmentId = role.SiteDepartmentId,
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
        }
    }
}