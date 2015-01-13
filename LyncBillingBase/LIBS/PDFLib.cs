using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

<<<<<<< HEAD
using ORM;
using ORM.Helpers;
=======
using CCC.ORM;
using CCC.ORM.Helpers;
using CCC.UTILS.Libs;
>>>>>>> 4d2825ed2d6c07fa47ef8a534e938e39e0b8f09c
using LyncBillingBase.Conf;
using LyncBillingBase.Reports;

namespace LyncBillingBase.Libs
{
    public class PDFLib
    {
        private static IElement titleParagraph { get; set; }
        private static Font titleFont = FontFactory.GetFont("Arial", 20, Font.BOLD);
        private static Font subTitleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
        private static Font headerCommentsFont = FontFactory.GetFont("Arial", 9, Font.ITALIC);
        private static Font boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
        private static Font endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
        private static Font bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);
        private static Font bodyFontSmall = FontFactory.GetFont("Arial", 10, Font.NORMAL);

        //Get the whole section with it's methods
        private static PDFReportColumnsDescriptionsSection ReportColumnsDescriptionsSection = 
            (PDFReportColumnsDescriptionsSection)ConfigurationManager.GetSection(PDFReportColumnsDescriptionsSection.ConfigurationSectionName);

        //Get the Report Columns Descriptions from the Configuration file.
        public static Dictionary<string, string> ReportColumnsDescriptions = ReportColumnsDescriptionsSection.PDFReportColumnsDescriptionsMap;


        public static Document InitializePDFDocument(HttpResponse response)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, response.OutputStream);
            document.Open();

            return document;
        }

        public static PdfPTable InitializePDFTable(int ColumnsCount, int[] widths)
        {
            //Create the actual data table
            PdfPTable pdfTable = new PdfPTable(ColumnsCount);
            pdfTable.HorizontalAlignment = 0;
            pdfTable.SpacingBefore = 30;
            pdfTable.SpacingAfter = 30;
            pdfTable.DefaultCell.Border = 0;
            //pdfTable.DefaultCell.Padding = 2;
            pdfTable.DefaultCell.PaddingBottom = 5;
            pdfTable.DefaultCell.PaddingTop = 5;
            pdfTable.DefaultCell.PaddingLeft = 2;
            pdfTable.DefaultCell.PaddingRight = 2;
            pdfTable.WidthPercentage = 100;
            if (widths.Length > 0 && widths.Length == ColumnsCount)
                pdfTable.SetWidths(widths);
            //else
            //    pdfTable.SetWidths(new int[] { 7, 4, 7, 4, 4 });

            return pdfTable;
        }

        public static Document AddPDFHeader(ref Document document, Dictionary<string, string> headers)
        {
            if (headers.Count > 0)
            {
                if (headers.ContainsKey("title"))
                {
                    Paragraph titleParagraph = new Paragraph("iBill | " + headers["title"], titleFont);
                    titleParagraph.SpacingAfter = 5;
                    document.Add(titleParagraph);
                }
                if (headers.ContainsKey("subTitle"))
                {
                    Paragraph subTitleParagraph;
                    if (headers.ContainsKey("siteName"))
                    {
                        subTitleParagraph = new Paragraph(headers["siteName"] + " | " + headers["subTitle"], subTitleFont);
                    }
                    else
                    {
                        subTitleParagraph = new Paragraph(headers["subTitle"], subTitleFont);
                    }
                    subTitleParagraph.SpacingAfter = 5;
                    document.Add(subTitleParagraph);
                }
                if (headers.ContainsKey("comments"))
                {
                    var commentsParagraph = new Paragraph(headers["comments"], headerCommentsFont);
                    commentsParagraph.SpacingBefore = 10;
                    commentsParagraph.SpacingAfter = 5;
                    document.Add(commentsParagraph);
                }
            }

            return document;
        }

        public static Document AddPDFTableContents(ref Document document, ref PdfPTable pdfTable, DataTable dt)
        {
            string cellText = string.Empty;

            foreach (DataColumn c in dt.Columns)
            {
                pdfTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(c.ColumnName), boldTableFont));
            }

            foreach (DataRow r in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Declare the pdfTable cell and fill it.
                    PdfPCell entryCell;

                    //Check if the cell being processed in not  empty nor null.
                    cellText = r[column.ColumnName].ToString();
                    if (string.IsNullOrEmpty(cellText))
                        cellText = "N/A";

                    //Format the cell text if it's the case of Duration
                    if (ReportColumnsDescriptionsSection.GetDescription(column.ColumnName) == "Duration" && cellText != "N/A")
                    {
                        entryCell = new PdfPCell(new Phrase(Convert.ToInt32(cellText).ConvertSecondsToReadable(), bodyFontSmall));
                    }
                    else
                    {
                        entryCell = new PdfPCell(new Phrase(cellText, bodyFontSmall));
                    }

                    //Set the cell padding, border configurations and then add it to the the pdfTable
                    entryCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                    entryCell.PaddingTop = 5;
                    entryCell.PaddingBottom = 5;
                    entryCell.PaddingLeft = 2;
                    entryCell.PaddingRight = 2;
                    pdfTable.AddCell(entryCell);
                }
            }

            // Add the Paragraph object to the document
            document.Add(pdfTable);
            return document;
        }

        public static Document AddPDFTableContents(ref Document document, ref PdfPTable pdfTable, DataTable dt, List<string> pdfColumnsSchema)
        {
            if (pdfColumnsSchema != null && pdfColumnsSchema.Count > 0)
            {
                foreach (string column in pdfColumnsSchema)
                {
                    if (dt.Columns.Contains(column))
                    {
                        pdfTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(column), boldTableFont));
                    }
                }

                foreach (DataRow r in dt.Rows)
                {
                    //foreach (DataColumn column in dt.Columns)
                    foreach (string column in pdfColumnsSchema)
                    {
                        //Declare the pdfTable cell and fill it.
                        PdfPCell entryCell;

                        //Format the cell text if it's the case of Duration
                        if (dt.Columns.Contains(column))
                        {
                            if (ReportColumnsDescriptionsSection.GetDescription(column) == "Duration")
                            {
                                entryCell = new PdfPCell(new Phrase(Convert.ToInt32(r[column]).ConvertSecondsToReadable(), bodyFontSmall));
                            }
                            else
                            {
                                string rowText = r[column].ToString();

                                if (string.IsNullOrEmpty(rowText))
                                    rowText = "N/A";

                                entryCell = new PdfPCell(new Phrase(rowText, bodyFontSmall));
                            }

                            //Set the cell padding, border configurations and then add it to the the pdfTable
                            entryCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                            entryCell.PaddingTop = 5;
                            entryCell.PaddingBottom = 5;
                            entryCell.PaddingLeft = 2;
                            entryCell.PaddingRight = 2;
                            pdfTable.AddCell(entryCell);
                        }
                    }
                }

                // Add the pdf-contents-table object to the document
                document.Add(pdfTable);
            }

            return document;
        }

        public static Document AddCombinedPDFTablesContents(ref Document document, DataTable dt, int[] pdfColumnsWidths, List<string> handles)
        {
            string cellText = string.Empty;
            DataRow[] selectedDataRows;
            string selectExpression = string.Empty;
            handles.Sort();

            if (handles != null && handles.Count > 0)
            {
                foreach (string handleItem in handles)
                {
                    PdfPTable pdfTable = InitializePDFTable(dt.Columns.Count, pdfColumnsWidths);
                    document.NewPage();

                    Paragraph pageTitleParagraph = new Paragraph(handleItem.Split('@')[0].ToUpper(), subTitleFont);
                    pageTitleParagraph.SpacingAfter = 25;
                    document.Add(pageTitleParagraph);

                    selectExpression = "SourceUserUri = '" + handleItem + "'";
                    selectedDataRows = dt.Select(selectExpression);

                    foreach (DataColumn c in dt.Columns)
                    {
                        pdfTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(c.ColumnName), boldTableFont));
                    }

                    foreach (DataRow r in selectedDataRows)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            //Declare the pdfTable cell and fill it.
                            PdfPCell entryCell;

                            //Check if the cell being processed in not  empty nor null.
                            cellText = r[column.ColumnName].ToString();
                            if (string.IsNullOrEmpty(cellText))
                                cellText = "N/A";

                            //Format the cell text if it's the case of Duration
                            if (ReportColumnsDescriptionsSection.GetDescription(column.ColumnName) == "Duration" && cellText != "N/A")
                            {
                                entryCell = new PdfPCell(new Phrase(Convert.ToInt32(cellText).ConvertSecondsToReadable(), bodyFontSmall));
                            }
                            else
                            {
                                entryCell = new PdfPCell(new Phrase(cellText, bodyFontSmall));
                            }

                            //Set the cell padding, border configurations and then add it to the the pdfTable
                            entryCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                            entryCell.PaddingTop = 5;
                            entryCell.PaddingBottom = 5;
                            entryCell.PaddingLeft = 2;
                            entryCell.PaddingRight = 2;
                            pdfTable.AddCell(entryCell);
                        }
                    }

                    // Add the Paragraph object to the document
                    document.Add(pdfTable);
                    selectExpression = string.Empty;
                    Array.Clear(selectedDataRows, 0, selectedDataRows.Length);
                }
            }
            return document;
        }

        public static Document AddPDFTableTotalsRow(ref Document document, Dictionary<string, object> totals, DataTable dt, int[] widths)
        {
            PdfPTable pdfTable = new PdfPTable(dt.Columns.Count);
            pdfTable.HorizontalAlignment = 0;
            pdfTable.DefaultCell.Border = 0;
            pdfTable.DefaultCell.PaddingBottom = 5;
            pdfTable.DefaultCell.PaddingTop = 5;
            pdfTable.DefaultCell.PaddingLeft = 2;
            pdfTable.DefaultCell.PaddingRight = 2;
            pdfTable.WidthPercentage = 100;
            if (widths.Length > 0 && widths.Length == dt.Columns.Count)
                pdfTable.SetWidths(widths);

            foreach (DataColumn column in dt.Columns)
            {
                if (dt.Columns[0].ColumnName == column.ColumnName)
                {
                    pdfTable.AddCell(new Phrase("Total", boldTableFont));
                }
                else if (totals.ContainsKey(column.ColumnName))
                {
                    pdfTable.AddCell(new Phrase(totals[column.ColumnName].ToString(), boldTableFont));
                }
                else
                {
                    pdfTable.AddCell(new Phrase(string.Empty, boldTableFont));
                }
            }

            document.Add(pdfTable);
            return document;
        }

        private static Document AddAccountingDetailedReportTotalsRow(ref Document document, UserCallsSummary userSummary)
        {
            PdfPTable pdfTable = new PdfPTable(5);
            pdfTable.HorizontalAlignment = 0;
            pdfTable.DefaultCell.Border = 0;
            pdfTable.DefaultCell.PaddingBottom = 5;
            pdfTable.DefaultCell.PaddingTop = 5;
            pdfTable.DefaultCell.PaddingLeft = 2;
            pdfTable.DefaultCell.PaddingRight = 2;
            pdfTable.WidthPercentage = 100;

            int[] widths = new int[] { 8, 5, 8, 8, 8 };
            pdfTable.SetWidths(widths);

            //int year = ((DateTime)extraParams["StartDate"]).YearNumber;
            //int startMonth = ((DateTime)extraParams["StartDate"]).Month;
            //int endMonth = ((DateTime)extraParams["EndDate"]).Month;

            //UsersCallsSummary userSummary = UsersCallsSummary.GetUserCallsSummary(sipAccount, year, startMonth, endMonth);

            //TOTALS HEADERS
            pdfTable.AddCell(new Phrase("Totals", boldTableFont));
            pdfTable.AddCell(new Phrase(string.Empty, boldTableFont));
            pdfTable.AddCell(new Phrase("Call Type", boldTableFont));
            pdfTable.AddCell(new Phrase("Cost", boldTableFont));
            pdfTable.AddCell(new Phrase("Duration", boldTableFont));
            pdfTable.CompleteRow();

            //Total Costs & Durations
            //Personal Calls Totals
            pdfTable.AddCell(new Phrase(string.Empty, bodyFont));
            pdfTable.AddCell(new Phrase(string.Empty, bodyFont));
            pdfTable.AddCell(new Phrase("Personal", bodyFont));
            pdfTable.AddCell(new Phrase(Decimal.Round(userSummary.PersonalCallsCost, 2).ToString(), bodyFontSmall));
            pdfTable.AddCell(new Phrase(userSummary.PersonalCallsDuration.ConvertSecondsToReadable().ToString(), bodyFontSmall));
            pdfTable.CompleteRow();  

            //Business Calls Totals
            pdfTable.AddCell(new Phrase(string.Empty, bodyFont));
            pdfTable.AddCell(new Phrase(string.Empty, bodyFont));
            pdfTable.AddCell(new Phrase("Business", bodyFont));
            pdfTable.AddCell(new Phrase(Decimal.Round(userSummary.BusinessCallsCost, 2).ToString(), bodyFontSmall));
            pdfTable.AddCell(new Phrase(userSummary.BusinessCallsDuration.ConvertSecondsToReadable().ToString(), bodyFontSmall));
            pdfTable.CompleteRow();

            //Unallocated Calls Totals
            pdfTable.AddCell(new Phrase(string.Empty, bodyFont));
            pdfTable.AddCell(new Phrase(string.Empty, bodyFont));
            pdfTable.AddCell(new Phrase("Unallocated", bodyFont));
            pdfTable.AddCell(new Phrase(Decimal.Round(userSummary.UnmarkedCallsCost, 2).ToString(), bodyFontSmall));
            pdfTable.AddCell(new Phrase(userSummary.UnmarkedCallsDuration.ConvertSecondsToReadable().ToString(), bodyFontSmall));
            pdfTable.CompleteRow();

            document.Add(pdfTable);
            return document;
        }

        public static void ClosePDFDocument(ref Document document)
        {
            document.Close();
        }


        //Ready-made reports functions.

        /**
         * Create Accounting Summary Report
         * 
         * @param response: Where to write the document
         * @param dt: The datatable which contains the data of the report.
         * @param totals: A dictionary of Phonecalls Data Totals such as Total Duration, Total Cost, Total Phonecalls.
         * @param pdfColumnsWidths: Array of integer numbers which specify the column widths of the report, from left to right.
         * @param pdfColumnsSchema: A list of column-names. This is to specify exactly what columns to include from the datatable in case we don't want to view all of its columns.
         * @param pdfDocumentHeaders: The header texts and paragraphs to be written on top of the main page.
         * 
         * @return @variable document of type Document.
         */
        public static Document CreateAccountingSummaryReport(HttpResponse ResponseStream, DataTable SourceDataTable, Dictionary<string, object> CallsCostsTotals, List<string> PDFColumnsSchema, int[] PDFColumnsWidths, Dictionary<string, string> PDFDocumentHeaders)
        {
            //----------------------------------
            //INITIALIZE THE REQUIRED VARIABLES
            //----------------------------------
            Document document;
            PdfWriter writer;
            PdfPTable pdfMainTable,
                pdfTotalsTable;
            Paragraph titleParagraph,
                subTitleParagraph,
                commentsParagraph;

            string cellText = string.Empty;


            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            //Exit the function in case the handles array is empty or the pdfColumnsSchema is either empty or it's size exceeds the DataTable's Columns number.
            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            if (ResponseStream == null ||
                SourceDataTable == null || SourceDataTable.Rows.Count == 0 ||
                CallsCostsTotals == null || CallsCostsTotals.Count == 0 ||
                PDFDocumentHeaders == null || PDFDocumentHeaders.Count == 0 ||
                PDFColumnsSchema == null || PDFColumnsSchema.Count == 0 || PDFColumnsSchema.Count > SourceDataTable.Columns.Count ||
                PDFColumnsWidths == null || PDFColumnsWidths.Length == 0 || PDFColumnsWidths.Length > SourceDataTable.Columns.Count)
            {
                return null;
            }


            //--------------------------------------------------
            //INITIALIZE THE PDF DOCUMENT
            //--------------------------------------------------
            document = new Document();
            writer = PdfWriter.GetInstance(document, ResponseStream.OutputStream);
            document.Open();


            //--------------------------------------------------
            //INITIALIZE THE PDF DOCUMENT TABLE
            //--------------------------------------------------
            pdfMainTable = new PdfPTable(PDFColumnsSchema.Count);
            pdfMainTable.HorizontalAlignment = 0;
            pdfMainTable.SpacingBefore = 30;
            pdfMainTable.SpacingAfter = 30;
            pdfMainTable.DefaultCell.Border = 0;
            //pdfMainTable.DefaultCell.Padding = 2;
            pdfMainTable.DefaultCell.PaddingBottom = 5;
            pdfMainTable.DefaultCell.PaddingTop = 5;
            pdfMainTable.DefaultCell.PaddingLeft = 2;
            pdfMainTable.DefaultCell.PaddingRight = 2;
            pdfMainTable.WidthPercentage = 100;
            if (PDFColumnsWidths.Length > 0 && PDFColumnsWidths.Length == PDFColumnsSchema.Count)
            {
                pdfMainTable.SetWidths(PDFColumnsWidths);
            }


            //--------------------------------------------------
            //INITIALIZE THE PDF DOCUMENT HEADER TEXTS
            //--------------------------------------------------
            if (PDFDocumentHeaders.ContainsKey("title"))
            {
                titleParagraph = new Paragraph("iBill | " + PDFDocumentHeaders["title"], titleFont);
                titleParagraph.SpacingAfter = 5;
                document.Add(titleParagraph);
            }
            if (PDFDocumentHeaders.ContainsKey("subTitle"))
            {
                if (PDFDocumentHeaders.ContainsKey("siteName"))
                {
                    subTitleParagraph = new Paragraph(PDFDocumentHeaders["siteName"] + " | " + PDFDocumentHeaders["subTitle"], subTitleFont);
                }
                else
                {
                    subTitleParagraph = new Paragraph(PDFDocumentHeaders["subTitle"], subTitleFont);
                }
                subTitleParagraph.SpacingAfter = 5;
                document.Add(subTitleParagraph);
            }
            if (PDFDocumentHeaders.ContainsKey("comments"))
            {
                commentsParagraph = new Paragraph(PDFDocumentHeaders["comments"], headerCommentsFont);
                commentsParagraph.SpacingBefore = 10;
                commentsParagraph.SpacingAfter = 5;
                document.Add(commentsParagraph);
            }


            //--------------------------------------------------
            //INITIALIZE THE MAIN CONTENT TABLE
            //--------------------------------------------------
            foreach (string column in PDFColumnsSchema)
            {
                if (SourceDataTable.Columns.Contains(column))
                {
                    pdfMainTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(column), boldTableFont));
                }
            }

            foreach (DataRow r in SourceDataTable.Rows)
            {
                //foreach (DataColumn column in dt.Columns)
                foreach (string column in PDFColumnsSchema)
                {
                    //Declare the pdfMainTable cell and fill it.
                    PdfPCell entryCell;

                    //Format the cell text if it's the case of Duration
                    if (SourceDataTable.Columns.Contains(column))
                    {
                        if (ReportColumnsDescriptionsSection.GetDescription(column) == GLOBALS.PhoneCallSummary.Duration.Description())
                        {
                            entryCell = new PdfPCell(new Phrase(Convert.ToInt32(r[column]).ConvertSecondsToReadable(), bodyFontSmall));
                        }
                        else
                        {
                            string rowText = r[column].ToString();

                            if (string.IsNullOrEmpty(rowText))
                                rowText = "N/A";

                            entryCell = new PdfPCell(new Phrase(rowText, bodyFontSmall));
                        }

                        //Set the cell padding, border configurations and then add it to the the pdfMainTable
                        entryCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                        entryCell.PaddingTop = 5;
                        entryCell.PaddingBottom = 5;
                        entryCell.PaddingLeft = 2;
                        entryCell.PaddingRight = 2;
                        pdfMainTable.AddCell(entryCell);
                    }
                }
            }
            // Add the pdf-contents-table object to the document
            document.Add(pdfMainTable);


            //--------------------------------------------------
            //ADD THE TOTALS AND SUMMATIONS ROWS AT THE END
            //--------------------------------------------------
            pdfTotalsTable = new PdfPTable(SourceDataTable.Columns.Count);
            pdfTotalsTable.HorizontalAlignment = 0;
            pdfTotalsTable.DefaultCell.Border = 0;
            pdfTotalsTable.DefaultCell.PaddingBottom = 5;
            pdfTotalsTable.DefaultCell.PaddingTop = 5;
            pdfTotalsTable.DefaultCell.PaddingLeft = 2;
            pdfTotalsTable.DefaultCell.PaddingRight = 2;
            pdfTotalsTable.WidthPercentage = 100;
            if (PDFColumnsWidths.Length > 0 && PDFColumnsWidths.Length == SourceDataTable.Columns.Count)
            {
                pdfTotalsTable.SetWidths(PDFColumnsWidths);
            }

            foreach (DataColumn column in SourceDataTable.Columns)
            {
                if (SourceDataTable.Columns[0].ColumnName == column.ColumnName)
                {
                    pdfTotalsTable.AddCell(new Phrase("Total", boldTableFont));
                }
                else if (CallsCostsTotals.ContainsKey(column.ColumnName))
                {
                    pdfTotalsTable.AddCell(new Phrase(CallsCostsTotals[column.ColumnName].ToString(), boldTableFont));
                }
                else
                {
                    pdfTotalsTable.AddCell(new Phrase(string.Empty, boldTableFont));
                }
            }
            //Add the totals table to the document.
            document.Add(pdfTotalsTable);


            //----------------------
            // CLOSE THE DOCUMENT
            //----------------------
            document.Close();


            //----------------------
            // RETURN THE DOCUMENT
            //----------------------
            return document;
        }


        /**
         * Create Accounting Detailed Report
         * 
         * @param response: Where to write the document
         * @param dt: The datatable which contains the data of the report.
         * @param pdfColumnsWidths: Array of integer numbers which specify the column widths of the report, from left to right.
         * @param pdfColumnsSchema: A list of column-names. This is to specify exactly what columns to include from the datatable in case we don't want to view all of its columns.
         * @param pdfDocumentHeaders: The header texts and paragraphs to be written on top of the main page.
         * @param handleName: How to divide the user data, say you pass "SourceUserId", then all the data will be divided based on the SourceUserId values, each collection on a separate page. This helps divides the listOfUsers phonecalls on pages.
         * @param UsersCollection: Collection of listOfUsers data, to optimize the performance and avoid getting it while creating the user report. This might cotnain some data fetched directly from the grid like Sip Accounts, Display Names, Summations and xtra Users Information.
         * 
         * @return @variable document of type Document.
         */
        public static Document CreateAccountingDetailedReport(HttpResponse ResponseStream, DataTable SourceDataTable, List<string> PDFColumnsSchema, int[] PDFColumnsWidths, Dictionary<string, string> PDFDocumentHeaders, string DataSeparatorName, Dictionary<string, Dictionary<string, object>> UsersInfoCollections, Dictionary<string, UserCallsSummary> UsersSummariesMap)
        {
            //----------------------------------
            //INITIALIZE THE REQUIRED VARIABLES
            //----------------------------------
            DataRow[] selectedDataRows;
            string selectExpression = string.Empty;
            string selectOrder = string.Empty;
            string pageTitleText = string.Empty;
            string cellText = string.Empty;
            Paragraph pageTitleParagraph;
            Paragraph pageSubTitleParagraph;
            ADUserInfo userInfo = new ADUserInfo();

            string employeeIDKey = GLOBALS.PhoneCallSummary.EmployeeID.Description();
            string displayNameKey = GLOBALS.PhoneCallSummary.DisplayName.Description();
            string sipAccountKey = GLOBALS.PhoneCallSummary.ChargingParty.Description();


            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            //Exit the function in case the handles array is empty or the pdfColumnsSchema is either empty or it's size exceeds the DataTable's Columns number.
            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            if (ResponseStream == null ||
                SourceDataTable == null || SourceDataTable.Rows.Count == 0 ||
                UsersSummariesMap == null || UsersSummariesMap.Count == 0 ||
                UsersInfoCollections == null || UsersInfoCollections.Count == 0 ||
                PDFDocumentHeaders == null || PDFDocumentHeaders.Count == 0 ||
                PDFColumnsSchema == null || PDFColumnsSchema.Count == 0 || PDFColumnsSchema.Count > SourceDataTable.Columns.Count)
            {
                return null;
            }


            //----------------------------------
            //INITIALIZE THE PDF DOCUMENT
            //----------------------------------
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, ResponseStream.OutputStream);
            document.Open();


            //----------------------------------
            //INITIALIZE THE HEADERS
            //----------------------------------
            if (PDFDocumentHeaders.Count > 0)
            {
                if (PDFDocumentHeaders.ContainsKey("title"))
                {
                    Paragraph titleParagraph = new Paragraph("iBill | " + PDFDocumentHeaders["title"], titleFont);
                    titleParagraph.SpacingAfter = 5;
                    document.Add(titleParagraph);
                }
                if (PDFDocumentHeaders.ContainsKey("subTitle"))
                {
                    Paragraph subTitleParagraph;
                    if (PDFDocumentHeaders.ContainsKey("siteName"))
                    {
                        subTitleParagraph = new Paragraph(PDFDocumentHeaders["siteName"] + " | " + PDFDocumentHeaders["subTitle"], subTitleFont);
                    }
                    else
                    {
                        subTitleParagraph = new Paragraph(PDFDocumentHeaders["subTitle"], subTitleFont);
                    }
                    subTitleParagraph.SpacingAfter = 5;
                    document.Add(subTitleParagraph);
                }
                if (PDFDocumentHeaders.ContainsKey("comments"))
                {
                    var commentsParagraph = new Paragraph(PDFDocumentHeaders["comments"], headerCommentsFont);
                    commentsParagraph.SpacingBefore = 10;
                    commentsParagraph.SpacingAfter = 5;
                    document.Add(commentsParagraph);
                }
            }


            //--------------------------------------
            //START CREATING THE REPORT DOCUMENT
            //--------------------------------------
            //start by sorting the handles array.
            List<string> SipAccounts = UsersInfoCollections.Keys.ToList();
            SipAccounts.Sort();

            //Begin the construction of the document.
            foreach (string sipAccount in SipAccounts)
            {
                PdfPTable pdfTable = InitializePDFTable(PDFColumnsSchema.Count, PDFColumnsWidths);
                document.NewPage();

                if (DataSeparatorName == sipAccountKey)
                {
                    string name = UsersInfoCollections[sipAccount][displayNameKey].ToString();
                    string groupNo = UsersInfoCollections[sipAccount][employeeIDKey].ToString();

                    if (string.IsNullOrEmpty(name))
                        name = sipAccount.ToLower();
                    if (!string.IsNullOrEmpty(groupNo))
                        groupNo = " [Group No. " + UsersInfoCollections[sipAccount][employeeIDKey].ToString() + "]";

                    pageTitleText = name + groupNo;
                }
                else
                {
                    pageTitleText = sipAccount;
                }

                pageTitleParagraph = new Paragraph(pageTitleText, subTitleFont);
                pageTitleParagraph.SpacingAfter = 10;
                document.Add(pageTitleParagraph);

                pageSubTitleParagraph = new Paragraph(PDFDocumentHeaders["subTitle"], subTitleFont);
                pageSubTitleParagraph.SpacingAfter = 20;
                document.Add(pageSubTitleParagraph);

                //Select the rows that are associated to the supplied handles
                if (PDFColumnsSchema.Contains("ResponseTime"))
                    selectOrder = "ResponseTime ASC";
                selectExpression = DataSeparatorName + " = '" + sipAccount + "'";
                selectedDataRows = SourceDataTable.Select(selectExpression, selectOrder);

                //Print the report table columns headers
                foreach (string column in PDFColumnsSchema)
                {
                    if (SourceDataTable.Columns.Contains(column))
                    {
                        pdfTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(column), boldTableFont));
                    }
                }

                //Bind the data cells to the respective columns
                foreach (DataRow r in selectedDataRows)
                {
                    foreach (string column in PDFColumnsSchema)
                    {
                        if (SourceDataTable.Columns.Contains(column))
                        {
                            //Declare the pdfTable cell and fill it.
                            PdfPCell entryCell;

                            //Check if the cell being processed in not  empty nor null.
                            cellText = r[column].ToString();
                            if (string.IsNullOrEmpty(cellText))
                                cellText = "N/A";

                            //Format the cell text if it's the case of Duration
                            if (ReportColumnsDescriptionsSection.GetDescription(column) == GLOBALS.PhoneCallSummary.Duration.Description() && cellText != "N/A")
                            {
                                entryCell = new PdfPCell(new Phrase(Convert.ToInt32(cellText).ConvertSecondsToReadable(),  bodyFontSmall));
                            }
                            else
                            {
                                entryCell = new PdfPCell(new Phrase(cellText, bodyFontSmall));
                            }

                            //Set the cell padding, border configurations and then add it to the the pdfTable
                            entryCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                            entryCell.PaddingTop = 5;
                            entryCell.PaddingBottom = 5;
                            entryCell.PaddingLeft = 2;
                            entryCell.PaddingRight = 2;
                            pdfTable.AddCell(entryCell);
                        }
                    }
                }

                // Add the Paragraph object to the document
                document.Add(pdfTable);

                if (UsersSummariesMap[sipAccount] != null)
                    AddAccountingDetailedReportTotalsRow(ref document, UsersSummariesMap[sipAccount]);

                selectExpression = string.Empty;
                selectOrder = string.Empty;
                Array.Clear(selectedDataRows, 0, selectedDataRows.Length);
            }


            //----------------------
            // CLOSE THE DOCUMENT
            //----------------------
            document.Close();


            //----------------------
            // RETURN THE DOCUMENT
            //----------------------
            return document;
        }


    }
}
