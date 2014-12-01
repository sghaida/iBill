using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;
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

            //AnnouncementsDataMapper AnnouncemenetsDM = new AnnouncementsDataMapper();
            //var announcementsForRole = AnnouncemenetsDM.GetAnnouncementsForRole(2);
            //var announcementsForSite = AnnouncemenetsDM.GetAnnouncementsForSite(1);

            //SystemRolesDataMapper SystemRolesDM = new SystemRolesDataMapper();
            //var systemRoles = SystemRolesDM.GetAll().ToList<SystemRole>();

            //DataAccess<SiteDepartment> sitesDepartements = new DataAccess<SiteDepartment>();

            //var allData = sitesDepartements.GetAllWithRelations();

            SitesDataMapper SitesMapper = new SitesDataMapper();
            GatewaysDataMapper GatewaysMapper = new GatewaysDataMapper();

            var MOA = SitesMapper.GetById(29);

            var gatewaysInfo = GatewaysMapper.GetAll(IncludeDataRelations: false);

            var allGatewaysInfo = GatewaysMapper.GetAll();

            var MOA_Gateways = GatewaysMapper.GetGatewaysForSite(MOA.ID); ;
        }

    }

}
