using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LyncBillingUI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session.Contents["UserData"] != null)
            {
                Response.Redirect(Global.APPLICATION_URL + "/User/Dashboard");
            }
            else
            {
                Response.Redirect(Global.APPLICATION_URL + "/Login");
            }
        }
    }
}