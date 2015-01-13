using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace LyncBillingBase.Conf
{
    public class PdfReportColumnDescriptionElement : ConfigurationElement
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

    public class PdfReportColumnsDescriptionsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PdfReportColumnDescriptionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PdfReportColumnDescriptionElement) element).Name;
        }
    }

    public class PdfReportColumnsDescriptionsSection : ConfigurationSection
    {
        public static string ConfigurationSectionName
        {
            get { return "PDFReportColumnsDescriptionsSection"; }
        }

        [ConfigurationProperty("PDFReportColumnsDescriptions")]
        public PdfReportColumnsDescriptionsCollection PdfReportColumnsDescriptions
        {
            get { return (PdfReportColumnsDescriptionsCollection) this["PDFReportColumnsDescriptions"]; }
        }

        public Dictionary<string, string> PdfReportColumnsDescriptionsMap
        {
            get
            {
                var columnsDescription = new Dictionary<string, string>();

                foreach (PdfReportColumnDescriptionElement element in PdfReportColumnsDescriptions)
                {
                    columnsDescription.Add(element.Name, element.Description);
                }

                return columnsDescription;
            }
        }

        public string GetDescription(string columnName)
        {
            if (PdfReportColumnsDescriptionsMap.Keys.Contains(columnName))
                return PdfReportColumnsDescriptionsMap[columnName];
            return columnName;
        }
    }
}