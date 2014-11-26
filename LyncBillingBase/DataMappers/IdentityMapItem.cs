using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DataMappers
{
    class IdentityMapItem<T> where T : class, new()
    {
        public T DataObject { get; set; }
        public DateTime AddedOn { get; set; }
        public bool Updated { get; set; }
        public bool DBSynced { get; set; }
        public readonly object MutexLock = new object();
    }
}
