using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace LyncBillingBase.Conf
{
    public class PdfReportPropertiesElement : ConfigurationElement
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


    public class PdfReportsPropertiesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PdfReportPropertiesElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PdfReportPropertiesElement) element).ReportName();
        }
    }


    public class PdfReportsPropertiesSection : ConfigurationSection
    {
        //The section name
        public static string ConfigurationSectionName
        {
            get { return "PDFReportsPropertiesSection"; }
        }

        [ConfigurationProperty("PDFReportsProperties")]
        public PdfReportsPropertiesCollection PdfReportsProperties
        {
            get { return (PdfReportsPropertiesCollection) this["PDFReportsProperties"]; }
        }

        public List<PdfReportPropertiesElement> PdfReportsPropertiesList
        {
            get
            {
                var reportsProperties = new List<PdfReportPropertiesElement>();

                foreach (PdfReportPropertiesElement element in PdfReportsProperties)
                {
                    reportsProperties.Add(element);
                }

                return reportsProperties;
            }
        }

        public Dictionary<string, Dictionary<string, object>> PdfReportsPropertiesMap
        {
            get
            {
                Dictionary<string, object> report;
                var reportsProperties = new Dictionary<string, Dictionary<string, object>>();

                foreach (PdfReportPropertiesElement element in PdfReportsProperties)
                {
                    report = new Dictionary<string, object>();
                    report.Add("columnsNames", element.ColumnsNames());
                    report.Add("columnsWidths", element.ColumnsWidths());

                    reportsProperties.Add(element.ReportName(), report);
                }

                return reportsProperties;
            }
        }

        public PdfReportPropertiesElement GetReportProperties(string reportName)
        {
            //Return the report if found, otherwise null
            return PdfReportsPropertiesList.SingleOrDefault(report => report.ReportName() == reportName);
        }
    }
}