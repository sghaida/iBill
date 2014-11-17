using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DAL
{
    public class MailTemplates
    {
        public int TemplateID { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateBody { get; set; }
    }
}
