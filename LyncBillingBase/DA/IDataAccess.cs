using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.DA
{
    public interface IDataAccess<T> where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        int Insert(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default);

        /// <summary>
        /// Update the data based on a predict expression 
        /// </summary>
        /// <param name="dataObject">Object to be updated</param>
        /// <param name="predicate">Expression<Func<T, bool>> predicate specify the expression that should be evaluated</param>
        /// <returns></returns>
        bool Update(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default);

        bool update(string sqlStatement);

        /// <summary>
        /// Delete Data from the repository
        /// </summary>
        /// <param name="dataObject">the object you wish to delete</param>
        /// <param name="where">Dictionary<string,object> Represents the where part that should be executed</param>
        /// <returns>bool status</returns>
        bool Delete(T dataObject, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default);

        bool Delete(string sqlStatemnet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(long id, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default);



        /// <summary>
        /// Gets the data from repository 
        /// </summary>
        /// <param name="fields">List<string> represents the fields that should be set</param>
        /// <param name="where">Dictionary<string,object> Represents the where part that should be executed</param>
        /// <param name="limit">Number of T objects to be populated</param>
        /// <returns>IQueryable<T>  Results</returns>
        IQueryable<T> Get(Dictionary<string, object> where, int limit = 25, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default);

        /// <summary>
        /// Gets the data from the repository and filter it based on the specified predicate expression
        /// </summary>
        /// <param name="predicate">Expression<Func<T, bool>> predicate specify the expression that should be evaluated</param>
        /// <returns>IQueryable<T>  Results</returns>
        IQueryable<T> Get(Expression<Func<T, bool>> predicate, string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default);

        IQueryable<T> Get(string sqlStatemnet);

        /// <summary>
        /// Get all the data from the Repo
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll(string dataSourceName = null, Enums.DataSources dataSource = Enums.DataSources.Default);

        

    }
}
