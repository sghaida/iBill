using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.CONF
{
    public class PDFReportPropertiesElement : ConfigurationElement
    {
        [ConfigurationProperty("reportName", IsKey = true, IsRequired = true)]
        private string reportName { get { return this["reportName"].ToString(); } }

        [ConfigurationProperty("columnsNames", IsRequired = true)]
        private string columnsNames { get { return this["columnsNames"].ToString(); } }

        [ConfigurationProperty("columnsWidths", IsRequired = true)]
        private string columnsWidths { get { return this["columnsWidths"].ToString(); } }

        public string ReportName()
        {
            return reportName.ToString();
        }

        public List<string> ColumnsNames()
        {
            return columnsNames.Split(',').ToList<string>();
        }

        public int[] ColumnsWidths()
        {
            int[] parsedWidths = { };
            List<string> originalWidthsValues = columnsWidths.Split(',').ToList<string>();

            parsedWidths = (from width in originalWidthsValues
                            select Convert.ToInt32(width)
                            ).ToArray<int>();

            return parsedWidths;
        }
    }


    public class PDFReportsPropertiesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PDFReportPropertiesElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PDFReportPropertiesElement)element).ReportName();
        }
    }


    public class PDFReportsPropertiesSection : ConfigurationSection
    {
        //The section name
        public static string ConfigurationSectionName { get { return "PDFReportsPropertiesSection"; } }

        [ConfigurationProperty("PDFReportsProperties")]
        public PDFReportsPropertiesCollection PDFReportsProperties
        {
            get { return (PDFReportsPropertiesCollection)this["PDFReportsProperties"]; }
        }

        public List<PDFReportPropertiesElement> PDFReportsPropertiesList
        {
            get
            {
                List<PDFReportPropertiesElement> reportsProperties = new List<PDFReportPropertiesElement>();

                foreach (PDFReportPropertiesElement element in PDFReportsProperties)
                {
                    reportsProperties.Add(element);
                }

                return reportsProperties;
            }
        }

        public Dictionary<string, Dictionary<string, object>> PDFReportsPropertiesMap
        {
            get
            {
                Dictionary<string, object> report;
                Dictionary<string, Dictionary<string, object>> reportsProperties = new Dictionary<string, Dictionary<string, object>>();

                foreach (PDFReportPropertiesElement element in PDFReportsProperties)
                {
                    report = new Dictionary<string, object>();
                    report.Add("columnsNames", element.ColumnsNames());
                    report.Add("columnsWidths", element.ColumnsWidths());

                    reportsProperties.Add(element.ReportName(), report);
                }

                return reportsProperties;
            }
        }

        public PDFReportPropertiesElement GetReportProperties(string reportName)
        {
            //Return the report if found, otherwise null
            return PDFReportsPropertiesList.SingleOrDefault<PDFReportPropertiesElement>(report => report.ReportName() == reportName);
        }
    }
}
