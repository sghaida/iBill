using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using CCC.ORM;
using CCC.ORM.Helpers;
using CCC.UTILS.Libs;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LyncBillingBase.Conf;
using LyncBillingBase.Reports;

namespace LyncBillingBase.Libs
{
    public class PdfLib
    {
        private static readonly Font TitleFont = FontFactory.GetFont("Arial", 20, Font.BOLD);
        private static readonly Font SubTitleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
        private static readonly Font HeaderCommentsFont = FontFactory.GetFont("Arial", 9, Font.ITALIC);
        private static readonly Font BoldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
        private static Font _endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
        private static readonly Font BodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);
        private static readonly Font BodyFontSmall = FontFactory.GetFont("Arial", 10, Font.NORMAL);
        private static IElement TitleParagraph { get; set; }

        public static Document InitializePdfDocument(HttpResponse response)
        {
            var document = new Document();
            var writer = PdfWriter.GetInstance(document, response.OutputStream);
            document.Open();

            return document;
        }

        public static PdfPTable InitializePdfTable(int columnsCount, int[] widths)
        {
            //Create the actual data table
            var pdfTable = new PdfPTable(columnsCount);
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
            if (widths.Length > 0 && widths.Length == columnsCount)
                pdfTable.SetWidths(widths);
            //else
            //    pdfTable.SetWidths(new int[] { 7, 4, 7, 4, 4 });

            return pdfTable;
        }

        public static Document AddPdfHeader(ref Document document, Dictionary<string, string> headers)
        {
            if (headers.Count > 0)
            {
                if (headers.ContainsKey("title"))
                {
                    var titleParagraph = new Paragraph("iBill | " + headers["title"], TitleFont);
                    titleParagraph.SpacingAfter = 5;
                    document.Add(titleParagraph);
                }
                if (headers.ContainsKey("subTitle"))
                {
                    Paragraph subTitleParagraph;
                    if (headers.ContainsKey("siteName"))
                    {
                        subTitleParagraph = new Paragraph(headers["siteName"] + " | " + headers["subTitle"],
                            SubTitleFont);
                    }
                    else
                    {
                        subTitleParagraph = new Paragraph(headers["subTitle"], SubTitleFont);
                    }
                    subTitleParagraph.SpacingAfter = 5;
                    document.Add(subTitleParagraph);
                }
                if (headers.ContainsKey("comments"))
                {
                    var commentsParagraph = new Paragraph(headers["comments"], HeaderCommentsFont);
                    commentsParagraph.SpacingBefore = 10;
                    commentsParagraph.SpacingAfter = 5;
                    document.Add(commentsParagraph);
                }
            }

            return document;
        }

        public static Document AddPDFTableContents(ref Document document, ref PdfPTable pdfTable, DataTable dt)
        {
            var cellText = string.Empty;

            foreach (DataColumn c in dt.Columns)
            {
                pdfTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(c.ColumnName), BoldTableFont));
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
                    if (ReportColumnsDescriptionsSection.GetDescription(column.ColumnName) == "Duration" &&
                        cellText != "N/A")
                    {
                        entryCell =
                            new PdfPCell(new Phrase(Convert.ToInt32(cellText).ConvertSecondsToReadable(), BodyFontSmall));
                    }
                    else
                    {
                        entryCell = new PdfPCell(new Phrase(cellText, BodyFontSmall));
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

        public static Document AddPDFTableContents(ref Document document, ref PdfPTable pdfTable, DataTable dt,
            List<string> pdfColumnsSchema)
        {
            if (pdfColumnsSchema != null && pdfColumnsSchema.Count > 0)
            {
                foreach (var column in pdfColumnsSchema)
                {
                    if (dt.Columns.Contains(column))
                    {
                        pdfTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(column),
                            BoldTableFont));
                    }
                }

                foreach (DataRow r in dt.Rows)
                {
                    //foreach (DataColumn column in dt.Columns)
                    foreach (var column in pdfColumnsSchema)
                    {
                        //Declare the pdfTable cell and fill it.
                        PdfPCell entryCell;

                        //Format the cell text if it's the case of Duration
                        if (dt.Columns.Contains(column))
                        {
                            if (ReportColumnsDescriptionsSection.GetDescription(column) == "Duration")
                            {
                                entryCell =
                                    new PdfPCell(new Phrase(Convert.ToInt32(r[column]).ConvertSecondsToReadable(),
                                        BodyFontSmall));
                            }
                            else
                            {
                                var rowText = r[column].ToString();

                                if (string.IsNullOrEmpty(rowText))
                                    rowText = "N/A";

                                entryCell = new PdfPCell(new Phrase(rowText, BodyFontSmall));
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

        public static Document AddCombinedPdfTablesContents(ref Document document, DataTable dt, int[] pdfColumnsWidths,
            List<string> handles)
        {
            var cellText = string.Empty;
            DataRow[] selectedDataRows;
            var selectExpression = string.Empty;
            handles.Sort();

            if (handles != null && handles.Count > 0)
            {
                foreach (var handleItem in handles)
                {
                    var pdfTable = InitializePdfTable(dt.Columns.Count, pdfColumnsWidths);
                    document.NewPage();

                    var pageTitleParagraph = new Paragraph(handleItem.Split('@')[0].ToUpper(), SubTitleFont);
                    pageTitleParagraph.SpacingAfter = 25;
                    document.Add(pageTitleParagraph);

                    selectExpression = "SourceUserUri = '" + handleItem + "'";
                    selectedDataRows = dt.Select(selectExpression);

                    foreach (DataColumn c in dt.Columns)
                    {
                        pdfTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(c.ColumnName),
                            BoldTableFont));
                    }

                    foreach (var r in selectedDataRows)
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
                            if (ReportColumnsDescriptionsSection.GetDescription(column.ColumnName) == "Duration" &&
                                cellText != "N/A")
                            {
                                entryCell =
                                    new PdfPCell(new Phrase(Convert.ToInt32(cellText).ConvertSecondsToReadable(),
                                        BodyFontSmall));
                            }
                            else
                            {
                                entryCell = new PdfPCell(new Phrase(cellText, BodyFontSmall));
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

        public static Document AddPdfTableTotalsRow(ref Document document, Dictionary<string, object> totals,
            DataTable dt, int[] widths)
        {
            var pdfTable = new PdfPTable(dt.Columns.Count);
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
                    pdfTable.AddCell(new Phrase("Total", BoldTableFont));
                }
                else if (totals.ContainsKey(column.ColumnName))
                {
                    pdfTable.AddCell(new Phrase(totals[column.ColumnName].ToString(), BoldTableFont));
                }
                else
                {
                    pdfTable.AddCell(new Phrase(string.Empty, BoldTableFont));
                }
            }

            document.Add(pdfTable);
            return document;
        }

        private static Document AddAccountingDetailedReportTotalsRow(ref Document document, UserCallsSummary userSummary)
        {
            var pdfTable = new PdfPTable(5);
            pdfTable.HorizontalAlignment = 0;
            pdfTable.DefaultCell.Border = 0;
            pdfTable.DefaultCell.PaddingBottom = 5;
            pdfTable.DefaultCell.PaddingTop = 5;
            pdfTable.DefaultCell.PaddingLeft = 2;
            pdfTable.DefaultCell.PaddingRight = 2;
            pdfTable.WidthPercentage = 100;

            int[] widths = {8, 5, 8, 8, 8};
            pdfTable.SetWidths(widths);

            //int year = ((DateTime)extraParams["StartDate"]).YearNumber;
            //int startMonth = ((DateTime)extraParams["StartDate"]).Month;
            //int endMonth = ((DateTime)extraParams["EndDate"]).Month;

            //UsersCallsSummary userSummary = UsersCallsSummary.GetUserCallsSummary(sipAccount, year, startMonth, endMonth);

            //TOTALS HEADERS
            pdfTable.AddCell(new Phrase("Totals", BoldTableFont));
            pdfTable.AddCell(new Phrase(string.Empty, BoldTableFont));
            pdfTable.AddCell(new Phrase("Call Type", BoldTableFont));
            pdfTable.AddCell(new Phrase("Cost", BoldTableFont));
            pdfTable.AddCell(new Phrase("Duration", BoldTableFont));
            pdfTable.CompleteRow();

            //Total Costs & Durations
            //Personal Calls Totals
            pdfTable.AddCell(new Phrase(string.Empty, BodyFont));
            pdfTable.AddCell(new Phrase(string.Empty, BodyFont));
            pdfTable.AddCell(new Phrase("Personal", BodyFont));
            pdfTable.AddCell(new Phrase(Decimal.Round(userSummary.PersonalCallsCost, 2).ToString(), BodyFontSmall));
            pdfTable.AddCell(new Phrase(userSummary.PersonalCallsDuration.ConvertSecondsToReadable(), BodyFontSmall));
            pdfTable.CompleteRow();

            //Business Calls Totals
            pdfTable.AddCell(new Phrase(string.Empty, BodyFont));
            pdfTable.AddCell(new Phrase(string.Empty, BodyFont));
            pdfTable.AddCell(new Phrase("Business", BodyFont));
            pdfTable.AddCell(new Phrase(Decimal.Round(userSummary.BusinessCallsCost, 2).ToString(), BodyFontSmall));
            pdfTable.AddCell(new Phrase(userSummary.BusinessCallsDuration.ConvertSecondsToReadable(), BodyFontSmall));
            pdfTable.CompleteRow();

            //Unallocated Calls Totals
            pdfTable.AddCell(new Phrase(string.Empty, BodyFont));
            pdfTable.AddCell(new Phrase(string.Empty, BodyFont));
            pdfTable.AddCell(new Phrase("Unallocated", BodyFont));
            pdfTable.AddCell(new Phrase(Decimal.Round(userSummary.UnmarkedCallsCost, 2).ToString(), BodyFontSmall));
            pdfTable.AddCell(new Phrase(userSummary.UnmarkedCallsDuration.ConvertSecondsToReadable(), BodyFontSmall));
            pdfTable.CompleteRow();

            document.Add(pdfTable);
            return document;
        }

        public static void ClosePdfDocument(ref Document document)
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

        public static Document CreateAccountingSummaryReport(HttpResponse responseStream, DataTable sourceDataTable,
            Dictionary<string, object> callsCostsTotals, List<string> pdfColumnsSchema, int[] pdfColumnsWidths,
            Dictionary<string, string> pdfDocumentHeaders)
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

            var cellText = string.Empty;


            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            //Exit the function in case the handles array is empty or the pdfColumnsSchema is either empty or it's size exceeds the DataTable's Columns number.
            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            if (responseStream == null ||
                sourceDataTable == null || sourceDataTable.Rows.Count == 0 ||
                callsCostsTotals == null || callsCostsTotals.Count == 0 ||
                pdfDocumentHeaders == null || pdfDocumentHeaders.Count == 0 ||
                pdfColumnsSchema == null || pdfColumnsSchema.Count == 0 ||
                pdfColumnsSchema.Count > sourceDataTable.Columns.Count ||
                pdfColumnsWidths == null || pdfColumnsWidths.Length == 0 ||
                pdfColumnsWidths.Length > sourceDataTable.Columns.Count)
            {
                return null;
            }


            //--------------------------------------------------
            //INITIALIZE THE PDF DOCUMENT
            //--------------------------------------------------
            document = new Document();
            writer = PdfWriter.GetInstance(document, responseStream.OutputStream);
            document.Open();


            //--------------------------------------------------
            //INITIALIZE THE PDF DOCUMENT TABLE
            //--------------------------------------------------
            pdfMainTable = new PdfPTable(pdfColumnsSchema.Count);
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
            if (pdfColumnsWidths.Length > 0 && pdfColumnsWidths.Length == pdfColumnsSchema.Count)
            {
                pdfMainTable.SetWidths(pdfColumnsWidths);
            }


            //--------------------------------------------------
            //INITIALIZE THE PDF DOCUMENT HEADER TEXTS
            //--------------------------------------------------
            if (pdfDocumentHeaders.ContainsKey("title"))
            {
                titleParagraph = new Paragraph("iBill | " + pdfDocumentHeaders["title"], TitleFont);
                titleParagraph.SpacingAfter = 5;
                document.Add(titleParagraph);
            }
            if (pdfDocumentHeaders.ContainsKey("subTitle"))
            {
                if (pdfDocumentHeaders.ContainsKey("siteName"))
                {
                    subTitleParagraph =
                        new Paragraph(pdfDocumentHeaders["siteName"] + " | " + pdfDocumentHeaders["subTitle"],
                            SubTitleFont);
                }
                else
                {
                    subTitleParagraph = new Paragraph(pdfDocumentHeaders["subTitle"], SubTitleFont);
                }
                subTitleParagraph.SpacingAfter = 5;
                document.Add(subTitleParagraph);
            }
            if (pdfDocumentHeaders.ContainsKey("comments"))
            {
                commentsParagraph = new Paragraph(pdfDocumentHeaders["comments"], HeaderCommentsFont);
                commentsParagraph.SpacingBefore = 10;
                commentsParagraph.SpacingAfter = 5;
                document.Add(commentsParagraph);
            }


            //--------------------------------------------------
            //INITIALIZE THE MAIN CONTENT TABLE
            //--------------------------------------------------
            foreach (var column in pdfColumnsSchema)
            {
                if (sourceDataTable.Columns.Contains(column))
                {
                    pdfMainTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(column),
                        BoldTableFont));
                }
            }

            foreach (DataRow r in sourceDataTable.Rows)
            {
                //foreach (DataColumn column in dt.Columns)
                foreach (var column in pdfColumnsSchema)
                {
                    //Declare the pdfMainTable cell and fill it.
                    PdfPCell entryCell;

                    //Format the cell text if it's the case of Duration
                    if (sourceDataTable.Columns.Contains(column))
                    {
                        if (ReportColumnsDescriptionsSection.GetDescription(column) ==
                            Globals.PhoneCallSummary.Duration.Description())
                        {
                            entryCell =
                                new PdfPCell(new Phrase(Convert.ToInt32(r[column]).ConvertSecondsToReadable(),
                                    BodyFontSmall));
                        }
                        else
                        {
                            var rowText = r[column].ToString();

                            if (string.IsNullOrEmpty(rowText))
                                rowText = "N/A";

                            entryCell = new PdfPCell(new Phrase(rowText, BodyFontSmall));
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
            pdfTotalsTable = new PdfPTable(sourceDataTable.Columns.Count);
            pdfTotalsTable.HorizontalAlignment = 0;
            pdfTotalsTable.DefaultCell.Border = 0;
            pdfTotalsTable.DefaultCell.PaddingBottom = 5;
            pdfTotalsTable.DefaultCell.PaddingTop = 5;
            pdfTotalsTable.DefaultCell.PaddingLeft = 2;
            pdfTotalsTable.DefaultCell.PaddingRight = 2;
            pdfTotalsTable.WidthPercentage = 100;
            if (pdfColumnsWidths.Length > 0 && pdfColumnsWidths.Length == sourceDataTable.Columns.Count)
            {
                pdfTotalsTable.SetWidths(pdfColumnsWidths);
            }

            foreach (DataColumn column in sourceDataTable.Columns)
            {
                if (sourceDataTable.Columns[0].ColumnName == column.ColumnName)
                {
                    pdfTotalsTable.AddCell(new Phrase("Total", BoldTableFont));
                }
                else if (callsCostsTotals.ContainsKey(column.ColumnName))
                {
                    pdfTotalsTable.AddCell(new Phrase(callsCostsTotals[column.ColumnName].ToString(), BoldTableFont));
                }
                else
                {
                    pdfTotalsTable.AddCell(new Phrase(string.Empty, BoldTableFont));
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

        public static Document CreateAccountingDetailedReport(HttpResponse responseStream, DataTable sourceDataTable,
            List<string> pdfColumnsSchema, int[] pdfColumnsWidths, Dictionary<string, string> pdfDocumentHeaders,
            string dataSeparatorName, Dictionary<string, Dictionary<string, object>> usersInfoCollections,
            Dictionary<string, UserCallsSummary> usersSummariesMap)
        {
            //----------------------------------
            //INITIALIZE THE REQUIRED VARIABLES
            //----------------------------------
            DataRow[] selectedDataRows;
            var selectExpression = string.Empty;
            var selectOrder = string.Empty;
            var pageTitleText = string.Empty;
            var cellText = string.Empty;
            Paragraph pageTitleParagraph;
            Paragraph pageSubTitleParagraph;
            var userInfo = new AdUserInfo();

            var employeeIdKey = Globals.PhoneCallSummary.EmployeeId.Description();
            var displayNameKey = Globals.PhoneCallSummary.DisplayName.Description();
            var sipAccountKey = Globals.PhoneCallSummary.ChargingParty.Description();


            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            //Exit the function in case the handles array is empty or the pdfColumnsSchema is either empty or it's size exceeds the DataTable's Columns number.
            //--------------------------------------------------------------------------------------------------------------------------------------------------------
            if (responseStream == null ||
                sourceDataTable == null || sourceDataTable.Rows.Count == 0 ||
                usersSummariesMap == null || usersSummariesMap.Count == 0 ||
                usersInfoCollections == null || usersInfoCollections.Count == 0 ||
                pdfDocumentHeaders == null || pdfDocumentHeaders.Count == 0 ||
                pdfColumnsSchema == null || pdfColumnsSchema.Count == 0 ||
                pdfColumnsSchema.Count > sourceDataTable.Columns.Count)
            {
                return null;
            }


            //----------------------------------
            //INITIALIZE THE PDF DOCUMENT
            //----------------------------------
            var document = new Document();
            var writer = PdfWriter.GetInstance(document, responseStream.OutputStream);
            document.Open();


            //----------------------------------
            //INITIALIZE THE HEADERS
            //----------------------------------
            if (pdfDocumentHeaders.Count > 0)
            {
                if (pdfDocumentHeaders.ContainsKey("title"))
                {
                    var titleParagraph = new Paragraph("iBill | " + pdfDocumentHeaders["title"], TitleFont);
                    titleParagraph.SpacingAfter = 5;
                    document.Add(titleParagraph);
                }
                if (pdfDocumentHeaders.ContainsKey("subTitle"))
                {
                    Paragraph subTitleParagraph;
                    if (pdfDocumentHeaders.ContainsKey("siteName"))
                    {
                        subTitleParagraph =
                            new Paragraph(pdfDocumentHeaders["siteName"] + " | " + pdfDocumentHeaders["subTitle"],
                                SubTitleFont);
                    }
                    else
                    {
                        subTitleParagraph = new Paragraph(pdfDocumentHeaders["subTitle"], SubTitleFont);
                    }
                    subTitleParagraph.SpacingAfter = 5;
                    document.Add(subTitleParagraph);
                }
                if (pdfDocumentHeaders.ContainsKey("comments"))
                {
                    var commentsParagraph = new Paragraph(pdfDocumentHeaders["comments"], HeaderCommentsFont);
                    commentsParagraph.SpacingBefore = 10;
                    commentsParagraph.SpacingAfter = 5;
                    document.Add(commentsParagraph);
                }
            }


            //--------------------------------------
            //START CREATING THE REPORT DOCUMENT
            //--------------------------------------
            //start by sorting the handles array.
            var sipAccounts = usersInfoCollections.Keys.ToList();
            sipAccounts.Sort();

            //Begin the construction of the document.
            foreach (var sipAccount in sipAccounts)
            {
                var pdfTable = InitializePdfTable(pdfColumnsSchema.Count, pdfColumnsWidths);
                document.NewPage();

                if (dataSeparatorName == sipAccountKey)
                {
                    var name = usersInfoCollections[sipAccount][displayNameKey].ToString();
                    var groupNo = usersInfoCollections[sipAccount][employeeIdKey].ToString();

                    if (string.IsNullOrEmpty(name))
                        name = sipAccount.ToLower();
                    if (!string.IsNullOrEmpty(groupNo))
                        groupNo = " [Group No. " + usersInfoCollections[sipAccount][employeeIdKey] + "]";

                    pageTitleText = name + groupNo;
                }
                else
                {
                    pageTitleText = sipAccount;
                }

                pageTitleParagraph = new Paragraph(pageTitleText, SubTitleFont);
                pageTitleParagraph.SpacingAfter = 10;
                document.Add(pageTitleParagraph);

                pageSubTitleParagraph = new Paragraph(pdfDocumentHeaders["subTitle"], SubTitleFont);
                pageSubTitleParagraph.SpacingAfter = 20;
                document.Add(pageSubTitleParagraph);

                //Select the rows that are associated to the supplied handles
                if (pdfColumnsSchema.Contains("ResponseTime"))
                    selectOrder = "ResponseTime ASC";
                selectExpression = dataSeparatorName + " = '" + sipAccount + "'";
                selectedDataRows = sourceDataTable.Select(selectExpression, selectOrder);

                //Print the report table columns headers
                foreach (var column in pdfColumnsSchema)
                {
                    if (sourceDataTable.Columns.Contains(column))
                    {
                        pdfTable.AddCell(new Phrase(ReportColumnsDescriptionsSection.GetDescription(column),
                            BoldTableFont));
                    }
                }

                //Bind the data cells to the respective columns
                foreach (var r in selectedDataRows)
                {
                    foreach (var column in pdfColumnsSchema)
                    {
                        if (sourceDataTable.Columns.Contains(column))
                        {
                            //Declare the pdfTable cell and fill it.
                            PdfPCell entryCell;

                            //Check if the cell being processed in not  empty nor null.
                            cellText = r[column].ToString();
                            if (string.IsNullOrEmpty(cellText))
                                cellText = "N/A";

                            //Format the cell text if it's the case of Duration
                            if (ReportColumnsDescriptionsSection.GetDescription(column) ==
                                Globals.PhoneCallSummary.Duration.Description() && cellText != "N/A")
                            {
                                entryCell =
                                    new PdfPCell(new Phrase(Convert.ToInt32(cellText).ConvertSecondsToReadable(),
                                        BodyFontSmall));
                            }
                            else
                            {
                                entryCell = new PdfPCell(new Phrase(cellText, BodyFontSmall));
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

                if (usersSummariesMap[sipAccount] != null)
                    AddAccountingDetailedReportTotalsRow(ref document, usersSummariesMap[sipAccount]);

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

        //Get the whole section with it's methods
        private static readonly PdfReportColumnsDescriptionsSection ReportColumnsDescriptionsSection =
            (PdfReportColumnsDescriptionsSection)
                ConfigurationManager.GetSection(PdfReportColumnsDescriptionsSection.ConfigurationSectionName);

        //Get the Report Columns Descriptions from the Configuration file.
        public static Dictionary<string, string> ReportColumnsDescriptions =
            ReportColumnsDescriptionsSection.PdfReportColumnsDescriptionsMap;
    }
}