using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LyncBillingBase.Repository
{
    interface IRepository<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        int Insert(T dataObject);

        /// <summary>
        /// Update the data based on a predict expression 
        /// </summary>
        /// <param name="dataObject">Object to be updated</param>
        /// <param name="predicate">Expression<Func<T, bool>> predicate specify the expression that should be evaluated</param>
        /// <returns></returns>
        bool Update(T dataObject);
       
        /// <summary>
        /// Delete Data from the repository
        /// </summary>
        /// <param name="dataObject">the object you wish to delete</param>
        /// <param name="where">Dictionary<string,object> Represents the where part that should be executed</param>
        /// <returns>bool status</returns>
        bool Delete(T dataObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(long id);
        


        /// <summary>
        /// Gets the data from repository 
        /// </summary>
        /// <param name="fields">List<string> represents the fields that should be set</param>
        /// <param name="where">Dictionary<string,object> Represents the where part that should be executed</param>
        /// <param name="limit">Number of T objects to be populated</param>
        /// <returns>IQueryable<T>  Results</returns>
        IQueryable<T> Get(Dictionary<string, object> where, int limit = 25);
        
        /// <summary>
        /// Gets the data from the repository and filter it based on the specified predicate expression
        /// </summary>
        /// <param name="predicate">Expression<Func<T, bool>> predicate specify the expression that should be evaluated</param>
        /// <returns>IQueryable<T>  Results</returns>
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get all the data from the Repo
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();
    }
}
