using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace CCC.UTILS
{
    public static class GLOBALS
    {
        public enum SpecialDateTime
        {
            [Description("1st Quarter")]
            [DefaultValue(1)]
            FirstQuarter,

            [Description("2nd Quarter")]
            [DefaultValue(2)]
            SecondQuarter,

            [Description("3rd Quarter")]
            [DefaultValue(3)]
            ThirdQuarter,

            [Description("4th Quarter")]
            [DefaultValue(4)]
            FourthQuarter,

            [Description("All Quarters")]
            [DefaultValue(5)]
            AllQuarters,

            [Description("One Year Ago from Today")]
            [DefaultValue(-1)]
            OneYearAgoFromToday,

            [Description("Two Years Ago from Today")]
            [DefaultValue(-2)]
            TwoYearsAgoFromToday
        }

    }

}
