using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyncBillingBase.Helpers;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "CallTypes", SourceType = Enums.DataSourceType.DBTable)]
    public class CallType
    {
        [IsIDField]
        [DbColumn("id")]
        public int ID { get; set; }

        [DbColumn("CallType")]
        public string Name { get; set; }


        /***
         * Custom functions
         */
        public static List<CallType> GetNGNCallTypes()
        {
            throw new NotImplementedException();
        }
    }
}
