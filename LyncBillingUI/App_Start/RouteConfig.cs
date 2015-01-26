using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace LyncBillingUI
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute(
                "AboutPageRoute",
                "About",
                "~/Pages/About.aspx");

            routes.MapPageRoute(
                "ContactPageRoute",
                "Contact",
                "~/Pages/Contact.aspx");

            routes.MapPageRoute(
                "UserDashboardPageRoute",
                "User/Dashboard",
                "~/Pages/User/Dashboard.aspx");

            routes.MapPageRoute(
                "UserManagePhoneCallsPageRoute",
                "User/Manage/Phonecalls",
                "~/Pages/User/ManagePhoneCalls.aspx");
        }
    }
}
