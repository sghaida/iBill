using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DA
{
    public interface IDistributedDataAccess<T> where T : class, new()
    {
        int Insert(string SQL);

        bool Update(string SQL);

        bool Delete(string SQL);

        IQueryable<T> Get(string SQL);
    }
}
