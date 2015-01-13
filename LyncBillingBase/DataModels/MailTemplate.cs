using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;






using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;


namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MailTemplates", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class MailTemplate : DataModel
    {
        [IsIDField]
        [DbColumn("TemplateID")]
        public int ID { get; set; }

        [AllowNull]
        [DbColumn("TemplateSubject")]
        public string Subject { get; set; }

        [AllowNull]
        [DbColumn("TemplateBody")]
        public string TemplateBody { get; set; }
    }
}
