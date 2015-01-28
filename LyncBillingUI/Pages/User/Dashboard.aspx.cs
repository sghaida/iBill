using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext;
using Ext.Net;

using CCC.ORM.Helpers;
using LyncBillingBase;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers;

namespace LyncBillingUI.Pages.User
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private static List<PhoneCall> phoneCalls;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                phoneCalls = Global.DATABASE.PhoneCalls.GetChargableCallsPerUser("aalhour@ccc.gr").ToList();
            }
        }

        protected void BusinessCallsCountLabel_Load(object sender, EventArgs e)
        {
            int count = phoneCalls.Where(item => item.UiCallType == LyncBillingGlobals.PhoneCalls.UiCallType.Business.Value()).Count();
            this.BusinessCallsCountLabel.Text = count.ToString();
        }

        protected void PersonalCallsCountLabel_Load(object sender, EventArgs e)
        {
            int count = phoneCalls.Where(item => item.UiCallType == LyncBillingGlobals.PhoneCalls.UiCallType.Personal.Value()).Count();
            this.PersonalCallsCountLabel.Text = count.ToString();
        }

        protected void UnallocatedCallsCountLabel_Load(object sender, EventArgs e)
        {
            int count = phoneCalls.Where(item => string.IsNullOrEmpty(item.UiCallType) || item.UiCallType == LyncBillingGlobals.PhoneCalls.UiCallType.Unallocated.Value()).Count();
            this.UnallocatedCallsCountLabel.Text = count.ToString();
        }
    }
}