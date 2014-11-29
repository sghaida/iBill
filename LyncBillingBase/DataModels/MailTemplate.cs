using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MailTemplates", SourceType = Enums.DataSourceType.DBTable, AccessType = Enums.DataSourceAccessType.SingleSource)]
    public class MailTemplate : DataModel
    {
        [IsIDField]
        [DbColumn("TemplateID")]
        public int TemplateID { get; set; }

        [AllowNull]
        [DbColumn("TemplateSubject")]
        public string TemplateSubject { get; set; }

        [AllowNull]
        [DbColumn("TemplateBody")]
        public string TemplateBody { get; set; }
    }
}
