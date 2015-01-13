using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace LyncBillingBase.Conf
{
    public class PDFReportPropertiesElement : ConfigurationElement
    {
        [ConfigurationProperty("reportName", IsKey = true, IsRequired = true)]
        private string reportName
        {
            get { return this["reportName"].ToString(); }
        }

        [ConfigurationProperty("columnsNames", IsRequired = true)]
        private string columnsNames
        {
            get { return this["columnsNames"].ToString(); }
        }

        [ConfigurationProperty("columnsWidths", IsRequired = true)]
        private string columnsWidths
        {
            get { return this["columnsWidths"].ToString(); }
        }

        public string ReportName()
        {
            return reportName;
        }

        public List<string> ColumnsNames()
        {
            return columnsNames.Split(',').ToList();
        }

        public int[] ColumnsWidths()
        {
            int[] parsedWidths = {};
            var originalWidthsValues = columnsWidths.Split(',').ToList();

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
            return ((PDFReportPropertiesElement) element).ReportName();
        }
    }


    public class PDFReportsPropertiesSection : ConfigurationSection
    {
        //The section name
        public static string ConfigurationSectionName
        {
            get { return "PDFReportsPropertiesSection"; }
        }

        [ConfigurationProperty("PDFReportsProperties")]
        public PDFReportsPropertiesCollection PDFReportsProperties
        {
            get { return (PDFReportsPropertiesCollection) this["PDFReportsProperties"]; }
        }

        public List<PDFReportPropertiesElement> PDFReportsPropertiesList
        {
            get
            {
                var reportsProperties = new List<PDFReportPropertiesElement>();

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
                var reportsProperties = new Dictionary<string, Dictionary<string, object>>();

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
            return PDFReportsPropertiesList.SingleOrDefault(report => report.ReportName() == reportName);
        }
    }
}