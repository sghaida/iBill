using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    /// <summary>
    /// This data model is obsolete. Use the PhoneCallExclusion
    /// </summary>

    [Obsolete]
    [DataSource(Name = "PhoneCallsExceptions", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class PhoneCallException
    {
        public int ID { get; set; }
        public string UserUri { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
    }
}
