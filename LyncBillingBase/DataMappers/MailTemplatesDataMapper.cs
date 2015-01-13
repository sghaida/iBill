using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class MailTemplatesDataMapper : DataAccess<MailTemplate>
    {
        private static List<MailTemplate> _mailTemplates = new List<MailTemplate>();

        public MailTemplatesDataMapper()
        {
            LoadMailTemplates();
        }

        private void LoadMailTemplates()
        {
            if (_mailTemplates == null || _mailTemplates.Count == 0)
            {
                _mailTemplates = base.GetAll().ToList();
            }
        }

        public override IEnumerable<MailTemplate> GetAll(string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            return _mailTemplates;
        }

        public override int Insert(MailTemplate dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var isContained = _mailTemplates.Contains(dataObject);
            var itExists =
                _mailTemplates.Exists(
                    item => item.Subject == dataObject.Subject && item.TemplateBody == dataObject.TemplateBody);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.Id = base.Insert(dataObject, dataSourceName, dataSourceType);
            _mailTemplates.Add(dataObject);

            return dataObject.Id;
        }

        public override bool Update(MailTemplate dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var template = _mailTemplates.Find(item => item.Id == dataObject.Id);

            if (template != null)
            {
                _mailTemplates.Remove(template);
                _mailTemplates.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(MailTemplate dataObject, string dataSourceName = null,
            Globals.DataSource.Type dataSourceType = Globals.DataSource.Type.Default)
        {
            var template = _mailTemplates.Find(item => item.Id == dataObject.Id);

            if (template != null)
            {
                _mailTemplates.Remove(template);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}