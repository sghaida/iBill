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
            
            //
            // Account Pages
            routes.MapPageRoute("", "Login", "~/Pages/Account/Login.aspx");
            routes.MapPageRoute("", "Logout", "~/Pages/Account/Logout.aspx");
            routes.MapPageRoute("", "Authorize", "~/Pages/Account/Authorize.aspx");

            //
            // Error Pages
            routes.MapPageRoute("404", "404", "~/Error/404.aspx");
            routes.MapPageRoute("Error", "Oops", "~/Error/Oops.aspx");

            //
            // User Pages
            routes.MapPageRoute("", "User/Bills", "~/Pages/User/Bills.aspx");
            routes.MapPageRoute("", "User/Dashboard", "~/Pages/User/Dashboard.aspx");
            routes.MapPageRoute("", "User/Statistics", "~/Pages/User/Statistics.aspx");
            routes.MapPageRoute("", "User/Phonecalls", "~/Pages/User/PhoneCalls.aspx");
            routes.MapPageRoute("", "User/AddressBook", "~/Pages/User/Addressbook.aspx");
            routes.MapPageRoute("", "User/TelephonyRates", "~/Pages/User/TelephonyRates.aspx");
            routes.MapPageRoute("", "User/History/PhoneCalls", "~/Pages/User/PhoneCallsHistory.aspx");

            //
            // Site Accountant Pages
            routes.MapPageRoute("", "Site/Accounting/Dashboard", "~/Pages/SiteAccounting/Dashboard.aspx");
            routes.MapPageRoute("", "Site/Accounting/DisputedCalls", "~/Pages/SiteAccounting/DisputedCalls.aspx");
        }
    }
}
