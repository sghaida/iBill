using System;
using System.Collections.Generic;
using CCC.UTILS.Helpers;

namespace CCC.UTILS.Libs
{
    public class SpecialDateTime
    {
        public int YearAsNumber { get; set; }
        public string YearAsText { get; set; }
        public int QuarterAsNumber { get; set; }
        public string QuarterAsText { get; set; }

        public static SpecialDateTime Get_OneYearAgoFromToday()
        {
            return new SpecialDateTime
            {
                YearAsText = Globals.SpecialDateTime.OneYearAgoFromToday.Description(),
                YearAsNumber = Convert.ToInt32(Globals.SpecialDateTime.OneYearAgoFromToday.Value())
            };
        }

        public static SpecialDateTime Get_TwoYearsAgoFromToday()
        {
            return new SpecialDateTime
            {
                YearAsText = Globals.SpecialDateTime.TwoYearsAgoFromToday.Description(),
                YearAsNumber = Convert.ToInt32(Globals.SpecialDateTime.TwoYearsAgoFromToday.Value())
            };
        }

        public static List<SpecialDateTime> GetQuartersOfTheYear()
        {
            var quarters = new List<SpecialDateTime>
            {
                //First Quarter
                new SpecialDateTime
                {
                    QuarterAsText = Globals.SpecialDateTime.FirstQuarter.Description(),
                    QuarterAsNumber = Convert.ToInt32(Globals.SpecialDateTime.FirstQuarter.Value())
                },
                //Second Quarter
                new SpecialDateTime
                {
                    QuarterAsText = Globals.SpecialDateTime.SecondQuarter.Description(),
                    QuarterAsNumber = Convert.ToInt32(Globals.SpecialDateTime.SecondQuarter.Value())
                },
                //Third Quarter
                new SpecialDateTime
                {
                    QuarterAsText = Globals.SpecialDateTime.ThirdQuarter.Description(),
                    QuarterAsNumber = Convert.ToInt32(Globals.SpecialDateTime.ThirdQuarter.Value())
                },
                //Fourth Quarter
                new SpecialDateTime
                {
                    QuarterAsText = Globals.SpecialDateTime.FourthQuarter.Description(),
                    QuarterAsNumber = Convert.ToInt32(Globals.SpecialDateTime.FourthQuarter.Value())
                },
                //All Quarters
                new SpecialDateTime
                {
                    QuarterAsText = Globals.SpecialDateTime.AllQuarters.Description(),
                    QuarterAsNumber = Convert.ToInt32(Globals.SpecialDateTime.AllQuarters.Value())
                }
            };

            return quarters;
        }

        public static string ConstructDateRange(int filterYear, int filterQuater, out DateTime startingDate,
            out DateTime endingDate)
        {
            int quarterStartingMonth, quarterEndingMonth;
            var finalDateRangeTitle = string.Empty;

            SpecialDateTime Quarter;
            var allQuarters = GetQuartersOfTheYear();

            var oneYearAgoFromToday = Get_OneYearAgoFromToday();
            var twoYearsAgoFromToday = Get_TwoYearsAgoFromToday();


            //Begin
            //First, handle the year
            if (filterYear == oneYearAgoFromToday.YearAsNumber)
            {
                startingDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                endingDate = DateTime.Now;

                finalDateRangeTitle = oneYearAgoFromToday.YearAsText;
            }
            else if (filterYear == twoYearsAgoFromToday.YearAsNumber)
            {
                startingDate = new DateTime(DateTime.Now.Year - 2, DateTime.Now.Month, 1);
                endingDate = DateTime.Now;

                finalDateRangeTitle = twoYearsAgoFromToday.YearAsText;
            }
            else
            {
                //Handle the fromMonth and toMonth
                switch (filterQuater)
                {
                    case 1:
                        quarterStartingMonth = 1;
                        quarterEndingMonth = 3;
                        break;

                    case 2:
                        quarterStartingMonth = 4;
                        quarterEndingMonth = 6;
                        break;

                    case 3:
                        quarterStartingMonth = 7;
                        quarterEndingMonth = 9;
                        break;

                    case 4:
                        quarterStartingMonth = 10;
                        quarterEndingMonth = 12;
                        break;

                    case 5:
                        quarterStartingMonth = 1;
                        quarterEndingMonth = 12;
                        break;

                    default:
                        quarterStartingMonth = 1;
                        quarterEndingMonth = 12;
                        break;
                }

                Quarter = allQuarters.Find(quarter => quarter.QuarterAsNumber == filterQuater) ??
                          allQuarters.Find(quarter => quarter.QuarterAsNumber == 5);

                startingDate = new DateTime(Convert.ToInt32(filterYear), quarterStartingMonth, 1);
                endingDate = new DateTime(Convert.ToInt32(filterYear), quarterEndingMonth, 1);

                finalDateRangeTitle = String.Format("{0} ({1})", filterYear, Quarter.QuarterAsText);
            }

            return finalDateRangeTitle;
        }

    }

}