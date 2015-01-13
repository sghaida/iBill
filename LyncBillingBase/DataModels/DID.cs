using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
using ORM;
using ORM.DataAccess;
using ORM.DataAttributes;
=======
using CCC.ORM;
using CCC.ORM.DataAccess;
using CCC.ORM.DataAttributes;
>>>>>>> 4d2825ed2d6c07fa47ef8a534e938e39e0b8f09c

namespace LyncBillingBase.DataModels
{
    [DataSource(Name = "DIDs", Type = GLOBALS.DataSource.Type.DBTable, AccessMethod = GLOBALS.DataSource.AccessMethod.SingleSource)]
    public class DID : DataModel
    {
        [IsIDField]
        [DbColumn("ID")]
        public int ID { get; set; }

        [DbColumn("Regex")]
        public string Regex { get; set; }

        [AllowNull]
        [DbColumn("Description")]
        public string Description { get; set; }

        [AllowNull]
        [DbColumn("SiteID")]
        public int SiteID { get; set; }


        //
        // Relations
        [DataRelation(WithDataModel = typeof(Site), OnDataModelKey = "ID", ThisKey = "SiteID")]
        public Site Site { get; set; }
    }
}
