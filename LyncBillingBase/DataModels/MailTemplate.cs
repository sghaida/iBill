using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MailTemplates", Type = Globals.DataSource.Type.DbTable,
        AccessMethod = Globals.DataSource.AccessMethod.SingleSource)]
    public class MailTemplate : DataModel
    {
        [IsIdField]
        [DbColumn("TemplateID")]
        public int Id { get; set; }

        [AllowNull]
        [DbColumn("TemplateSubject")]
        public string Subject { get; set; }

        [AllowNull]
        [DbColumn("TemplateBody")]
        public string TemplateBody { get; set; }
    }
}