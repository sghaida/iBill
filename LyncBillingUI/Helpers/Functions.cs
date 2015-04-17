using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LyncBillingUI.Helpers
{
    public static class Functions
    {
        //
        // System Roles Names - Lookup variables
        public static string SystemAdminRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SystemAdminRoleID); } }
        public static string SiteAdminRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteAdminRoleID); } }
        public static string SiteAccountantRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteAccountantRoleID); } }
        public static string DepartmentHeadRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.DepartmentHeadRoleID); } }

        //
        // Delegee Roles Names - Lookup variables
        public static string UserDelegeeRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.UserDelegeeRoleID); } }
        public static string DepartmentDelegeeRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.DepartmentDelegeeRoleID); } }
        public static string SiteDelegeeRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.SiteDelegeeRoleID); } }

        //
        // Normal User Role - Lookup variable
        public static string NormalUserRoleName { get { return Global.DATABASE.Roles.GetRoleNameById(Global.DATABASE.Roles.UserRoleID); } }


        //
        // Ext.NET UI Message Construction
        public static void Message(string title, string msg, string type, int hideDelay = 15000, bool isPinned = false, int width = 250, int height = 150)
        {
            NotificationConfig notificationConfig = new NotificationConfig();

            notificationConfig.Title = title;
            notificationConfig.Html = msg;

            //Hiding Delay in mlseconds
            notificationConfig.HideDelay = hideDelay;

            //Height and Width
            notificationConfig.Width = width;
            notificationConfig.Height = height;

            //Type
            if (type == "success")
                notificationConfig.Icon = Icon.Accept;
            else if (type == "info")
                notificationConfig.Icon = Icon.Information;
            else if (type == "warning")
                notificationConfig.Icon = Icon.AsteriskYellow;
            else if (type == "error")
                notificationConfig.Icon = Icon.Exclamation;
            else if (type == "help")
                notificationConfig.Icon = Icon.Help;

            //Pinning
            if (isPinned)
            {
                notificationConfig.ShowPin = true;
                notificationConfig.Pinned = true;
                notificationConfig.PinEvent = "click";
            }

            notificationConfig.BodyStyle = "background-color: #f9f9f9;";

            Notification.Show(notificationConfig);
        }

    }

}