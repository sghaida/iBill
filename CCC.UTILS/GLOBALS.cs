using System.ComponentModel;

namespace CCC.UTILS
{
    public static class Globals
    {
        public enum SpecialDateTime
        {
            [Description("1st Quarter")] [DefaultValue(1)] FirstQuarter,

            [Description("2nd Quarter")] [DefaultValue(2)] SecondQuarter,

            [Description("3rd Quarter")] [DefaultValue(3)] ThirdQuarter,

            [Description("4th Quarter")] [DefaultValue(4)] FourthQuarter,

            [Description("All Quarters")] [DefaultValue(5)] AllQuarters,

            [Description("One Year Ago from Today")] [DefaultValue(-1)] OneYearAgoFromToday,

            [Description("Two Years Ago from Today")] [DefaultValue(-2)] TwoYearsAgoFromToday
        }

        // The Data Relation GLOBALS
        public static class DataRelation
        {
            public enum Type
            {
                [Description("The intersection of two data models. Equivalent to an SQL INNER JOIN.")] [DefaultValue("INTERSECTION")] Intersection = 0,

                [Description("The union of two data models. Equivalent to an SQL OUTER JOIN.")] [DefaultValue("UNION")] Union = 1
            }
        }
    }
}