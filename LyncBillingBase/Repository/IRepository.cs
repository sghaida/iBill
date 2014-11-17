using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LyncBillingBase.Repository
{
    interface IRepository<T>
    {
        int Insert(T dataObject);
        bool Update(T dataObject);
        bool Delete(T dataObject);

        T GetById(long id);
        IQueryable<T> GetAll();
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
    }
}
