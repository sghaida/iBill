using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MailTemplates", SourceType = Enums.DataSourceType.DBTable)]
    public class MailTemplate
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
