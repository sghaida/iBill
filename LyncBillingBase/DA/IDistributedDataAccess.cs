using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DA
{
    public interface IDistributedDataAccess<T> where T : class, new()
    {

        public int Insert(string SQL);

        public bool Update(string SQL);

        public bool Delete(string SQL);

        public IQueryable<T> Get(string SQL);

    }
}
