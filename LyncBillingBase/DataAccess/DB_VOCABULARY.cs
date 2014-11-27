using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataAccess
{
    public static class DB_VOCABULARY
    {
        //RelationType
        public static string RELATION_TYPE
        {
            get
            {
                return "RelationType";
            }
        }

        //ThisKey
        public static string THIS_KEY
        {
            get
            {
                return "ThisKey";
            }
        }

        //TargetKey
        public static string TARGET_KEY
        {
            get
            {
                return "TargetKey";
            }
        }

        public static string SELECT_COLUMNS
        {
            get
            {
                return "SelectColumns";
            }
        }
    }
}
