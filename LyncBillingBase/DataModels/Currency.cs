using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ORM;
using ORM.DataAccess;
using ORM.DataAttributes;

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "NEW_Currencies", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class Currency : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("Name")]
        public string Name { get; set; }

        [DbColumn("ISO3Code")]
        public string ISO3Code { get; set; }
    }
}
