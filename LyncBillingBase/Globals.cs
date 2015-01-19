using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase
{
    /// <summary>
    /// LyncBillingBase specific globals
    /// </summary>
    public static class LyncBillingGlobals
    {
        /// <summary>
        /// The Chart-Report's Globals
        /// </summary>
        public static class ChartReport
        {
            public enum Name
            {
                [DefaultValue("Business")]
                [Description("Business Chart Report")]
                Business,

                [DefaultValue("Personal")]
                [Description("Personal Chart Report")]
                Personal,

                [DefaultValue("Unallocated")]
                [Description("Unallocated Chart Report")]
                Unallocated
            }
        }

    }

}
