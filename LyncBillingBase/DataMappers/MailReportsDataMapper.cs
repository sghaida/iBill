using System;
using System.Collections.Generic;
using System.Linq;
using CCC.ORM.DataAccess;
using CCC.ORM.Helpers;
using CCC.UTILS.Libs;
using LyncBillingBase.DataModels;
using LyncBillingBase.DataMappers.SQLQueries;

namespace LyncBillingBase.DataMappers
{
    public class MailReportsDataMapper : DataAccess<MailReport>
    {
        /***
         * THE SQL STATMENTS CONTAINER
         */
        MailReportsSQL _sqlStatments = new MailReportsSQL();


        /// <summary>
        /// Given an enumerable collection of MailReport objects, group the objects by EmailAddress and calculate the totals of them all.
        /// </summary>
        /// <param name="mailReports">Enumerable Collection of MailReport.</param>
        private static void GroupByUser(ref IEnumerable<MailReport> mailReports)
        {
            if(mailReports.Any())
            {
                mailReports = (
                    from report in mailReports
                    group report by new { report.EmailAddress }
                        into result
                        select new MailReport
                        {
                            Id = 0,
                            EmailAddress = result.Key.EmailAddress,
                            ReceivedCount = result.Sum(item => item.ReceivedCount),
                            ReceivedSize = result.Sum(item => item.ReceivedSize),
                            SentCount = result.Sum(item => item.SentCount),
                            SentSize = result.Sum(item => item.SentSize)
                        })
                    .OrderBy(item => item.ReportDate);
            }
        }

        /// <summary>
        /// Given a User's SipAccount, return all the mail reports for the specified yearAndMonth DateTime.
        /// If the date and time range was not specified, a default one is contructed by taking DateTime.Now.Year and DateTime.Now.Month.
        /// </summary>
        /// <param name="sipAccount">The User's SipAccount</param>
        /// <param name="yearAndMonth">Optional. The year and month of the report.</param>
        /// <returns>List of MailReport objects.</returns>
        public List<MailReport> GetByUser(string sipAccount, DateTime? yearAndMonth = null)
        {
            string startingDate, endingDate;
            Dictionary<string, object> whereConditions = new Dictionary<string, object>();

            if (yearAndMonth == null || yearAndMonth == DateTime.MinValue)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                var tempStartingDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));

                startingDate = tempStartingDate.ConvertDate(true);
                endingDate = tempStartingDate.AddMonths(1).ConvertDate(true);
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                DateTime specificDate = (DateTime)yearAndMonth;
                var tempStartingDate = specificDate.AddDays(-(specificDate.Day - 1));

                startingDate = tempStartingDate.ConvertDate(true);
                endingDate = tempStartingDate.AddMonths(1).ConvertDate(true);
            }

            try
            {
                whereConditions.Add("EmailAddress", sipAccount);
                whereConditions.Add("TimeStamp", String.Format("BETWEEN '{0}' AND '{1}'", startingDate, endingDate));

                return base.Get(whereConditions, 0).ToList();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a User's SipAccount, return a total mail report for the specified yearAndMonth DateTime.
        /// If the date and time range was not specified, a default one is contructed by taking DateTime.Now.Year and DateTime.Now.Month.
        /// </summary>
        /// <param name="sipAccount">The User's SipAccount</param>
        /// <param name="yearAndMonth">Optional. The year and month of the report.</param>
        /// <returns>A MailReport object.</returns>
        public MailReport GetTotalByUser(string sipAccount, DateTime? yearAndMonth = null)
        {
            DateTime reportDate;
            MailReport report = new MailReport();

            if (yearAndMonth == null || yearAndMonth == DateTime.MinValue)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                reportDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                var specificDate = (DateTime)yearAndMonth;
                reportDate = specificDate.AddDays(-(specificDate.Day - 1));
            }

            try
            {
                IEnumerable<MailReport> userMailReports = this.GetByUser(sipAccount, yearAndMonth) ?? (new List<MailReport>());

                if(userMailReports.Any())
                {
                    GroupByUser(ref userMailReports);
                    report = userMailReports.First();
                    report.ReportDate = reportDate;
                }

                return report;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Given a Site Name, a Department Name, return a total mail report for the specified yearAndMonth Date and Time range.
        /// If the date and time range was not specified, a default one is contructed by taking DateTime.Now.Year and DateTime.Now.Month.
        /// </summary>
        /// <param name="siteName">The Department's Site Name.</param>
        /// <param name="departmentName">The Department Name.</param>
        /// <param name="yearAndMonth">Optional. The year and month of the report.</param>
        /// <returns>A MailReport object.</returns>
        public MailReport GetByDepartment(string siteName, string departmentName, DateTime? yearAndMonth = null)
        {
            DateTime reportDate;
            string startingDate, endingDate;
            MailReport report = new MailReport();

            if (yearAndMonth == null || yearAndMonth == DateTime.MinValue)
            {
                //Both starting date and ending date respectively point to the beginning and ending of this current month.
                reportDate = DateTime.Now.AddDays(-(DateTime.Today.Day - 1));

                startingDate = reportDate.ConvertDate(true);
                endingDate = reportDate.AddMonths(1).ConvertDate(true);
            }
            else
            {
                //Assign the beginning of date.Month to the startingDate and the end of it to the endingDate 
                DateTime specificDate = (DateTime)yearAndMonth;
                reportDate = specificDate.AddDays(-(specificDate.Day - 1));

                startingDate = reportDate.ConvertDate(true);
                endingDate = reportDate.AddMonths(1).ConvertDate(true);
            }

            try
            {
                var sql = _sqlStatments.GetMailReportsForDepartment(
                    siteName,
                    departmentName,
                    startingDate,
                    endingDate
                );

                var reports = base.GetAll(sql) ?? (new List<MailReport>() as IEnumerable<MailReport>);

                if(reports.Any())
                {
                    report = reports.ToList().First();
                    report.ReportDate = reportDate;
                }

                return report;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }

}
