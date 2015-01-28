using System;
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "MonitoringServersInfo", Type = Globals.DataSource.Type.DbTable, AccessMethod = Globals.DataSource.AccessMethod.DistributedSource)]
    public class ChartReport : DataModel
    {
        [IsIdField]
        [DbColumn("Name")]
        public string Name { get; set; }

        [DbColumn("TotalCalls")]
        public long TotalCalls { get; set; }

        [DbColumn("TotalDuration")]
        public long TotalDuration { get; set; }

        [DbColumn("TotalCost")]
        public decimal TotalCost { get; set; }


        /// <summary>
        /// Must exist due to the existence of a base constructor
        /// </summary>
        public ChartReport() : base() { }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="chartReportName">The Name field value.</param>
        public ChartReport(string chartReportName)
        {
            this.Name = chartReportName;
        }

    }

}
