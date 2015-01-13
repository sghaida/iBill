using System.Collections.Generic;
using System.Linq;
using CCC.ORM;
using CCC.ORM.DataAccess;
using LyncBillingBase.DataModels;

namespace LyncBillingBase.DataMappers
{
    public class MailTemplatesDataMapper : DataAccess<MailTemplate>
    {
        private static List<MailTemplate> _MailTemplates = new List<MailTemplate>();

        public MailTemplatesDataMapper()
        {
            LoadMailTemplates();
        }

        private void LoadMailTemplates()
        {
            if (_MailTemplates == null || _MailTemplates.Count == 0)
            {
                _MailTemplates = base.GetAll().ToList();
            }
        }

        public override IEnumerable<MailTemplate> GetAll(string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            return _MailTemplates;
        }

        public override int Insert(MailTemplate dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var isContained = _MailTemplates.Contains(dataObject);
            var itExists =
                _MailTemplates.Exists(
                    item => item.Subject == dataObject.Subject && item.TemplateBody == dataObject.TemplateBody);

            if (isContained || itExists)
            {
                return -1;
            }
            dataObject.ID = base.Insert(dataObject, dataSourceName, dataSourceType);
            _MailTemplates.Add(dataObject);

            return dataObject.ID;
        }

        public override bool Update(MailTemplate dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var template = _MailTemplates.Find(item => item.ID == dataObject.ID);

            if (template != null)
            {
                _MailTemplates.Remove(template);
                _MailTemplates.Add(dataObject);

                return base.Update(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }

        public override bool Delete(MailTemplate dataObject, string dataSourceName = null,
            GLOBALS.DataSource.Type dataSourceType = GLOBALS.DataSource.Type.Default)
        {
            var template = _MailTemplates.Find(item => item.ID == dataObject.ID);

            if (template != null)
            {
                _MailTemplates.Remove(template);

                return base.Delete(dataObject, dataSourceName, dataSourceType);
            }
            return false;
        }
    }
}