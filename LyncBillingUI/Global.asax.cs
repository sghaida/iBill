using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

using LyncBillingBase.Repository;
using LyncBillingUI.Account;

namespace LyncBillingUI
{
    public class Global : HttpApplication
    {
        public static string APPLICATION_URL {get; set;}
        public static DataStorage DATABASE { get; set; }
        public static Encryption ENCRYPTION { get; set; }

        void Application_Start(object sender, EventArgs e)
        {
            //
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //
            // Instantiate the APPLICATION_URL to an empty string
            APPLICATION_URL = string.Empty;

            //
            // Initialize the ENCRYPTION variable
            ENCRYPTION = new Encryption();

            //
            // Instantiate the DATABASE instance
            DATABASE = DataStorage.Instance;
        }
    }
}