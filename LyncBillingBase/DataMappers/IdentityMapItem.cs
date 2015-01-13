using System;

namespace LyncBillingBase.DataMappers
{
    internal class IdentityMapItem<T> where T : class, new()
    {
        public readonly object MutexLock = new object();
        public T DataObject { get; set; }
        public DateTime AddedOn { get; set; }
        public bool Updated { get; set; }
        public bool DBSynced { get; set; }
    }
}