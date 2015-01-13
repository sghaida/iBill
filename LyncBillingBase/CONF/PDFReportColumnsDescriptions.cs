using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace LyncBillingBase.Conf
{
    public class PDFReportColumnDescriptionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
        }

        [ConfigurationProperty("description", IsRequired = true)]
        public string Description
        {
            get { return this["description"].ToString(); }
        }
    }

    public class PDFReportColumnsDescriptionsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PDFReportColumnDescriptionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PDFReportColumnDescriptionElement) element).Name;
        }
    }

    public class PDFReportColumnsDescriptionsSection : ConfigurationSection
    {
        public static string ConfigurationSectionName
        {
            get { return "PDFReportColumnsDescriptionsSection"; }
        }

        [ConfigurationProperty("PDFReportColumnsDescriptions")]
        public PDFReportColumnsDescriptionsCollection PDFReportColumnsDescriptions
        {
            get { return (PDFReportColumnsDescriptionsCollection) this["PDFReportColumnsDescriptions"]; }
        }

        public Dictionary<string, string> PDFReportColumnsDescriptionsMap
        {
            get
            {
                var columnsDescription = new Dictionary<string, string>();

                foreach (PDFReportColumnDescriptionElement element in PDFReportColumnsDescriptions)
                {
                    columnsDescription.Add(element.Name, element.Description);
                }

                return columnsDescription;
            }
        }

        public string GetDescription(string columnName)
        {
            if (PDFReportColumnsDescriptionsMap.Keys.Contains(columnName))
                return PDFReportColumnsDescriptionsMap[columnName];
            return columnName;
        }
    }
}