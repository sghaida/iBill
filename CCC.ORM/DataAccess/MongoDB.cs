using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;


namespace CCC.ORM.DataAccess
{
    public class MongoDb
    {
        private MongoDatabase _db;

        public MongoDatabase Database { get; private set; }

        public MongoDb(string connectionString, string dbname)
        {
            MongoClient client = new MongoClient(connectionString);
            var server = client.GetServer();
            Database = _db = server.GetDatabase(dbname);
        }

        /// <summary>
        /// Get current collection
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <returns>The current collection</returns>
        private MongoCollection<T> GetCollection<T>() where T : MongoDbObject
        {
            return _db.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// Count all the rows in the T collection
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <returns>The total of rows of the T collection</returns>
        public int Count<T>() where T : MongoDbObject
        {
           return Find<T>().Count<T>();            
        }

        /// <summary>
        /// Count all the rows in the T collection
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="exp">Mongo LINQ expression</param>
        /// <returns>The total of rows of the query</returns>
        public int Count<T>(Expression<Func<T, bool>> exp) where T : MongoDbObject
        {
            return Find(exp).Count();
        }


        /// <summary>
        /// Find all elements of a collection
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <returns>returns all the elements of a collection</returns>
        public IEnumerable<T> Find<T>() where T : MongoDbObject
        {
            return GetCollection<T>().FindAll();
        }

        /// <summary>
        /// Find with parameters
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="exp">Mongo LINQ expression</param>
        /// <returns>returns a Collection</returns>
        public IEnumerable<T> Find<T>(Expression<Func<T, bool>> exp) where T : MongoDbObject
        {
            return GetCollection<T>().AsQueryable<T>().Where(exp).ToList();
        }

        /// <summary>
        ///  Find a single result 
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="exp">Mongo LINQ expression</param>
        /// <returns>returns a single result</returns>
        public T FindOne<T>(Expression<Func<T, bool>> exp) where T : MongoDbObject
        {
            // TODO more than one result throws an exception
            return Find(exp).SingleOrDefault();
        }

        /// <summary>
        /// Insert Single entity.
        /// If you wanna insert several entities in a row, please use InsertBatch because it is faster for multiple insertion.
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="entity">Entity name</param>
        public void Insert<T>(T entity) where T : MongoDbObject
        {
            if (entity != null)
            {
                GetCollection<T>().Insert(entity);
            }
        }

        /// <summary>
        /// Insert in batch
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="entities">ICollection of Entities of T typeof</param>
        public void Insert<T>(ICollection<T> entities) where T : MongoDbObject
        {
            if (entities != null)
            {
                GetCollection<T>().InsertBatch(entities);
            }
        }

        /// <summary>
        /// Update a given Entity. 
        /// If the Id member of the document has a value, Save calls Update, otherwise it is assumed to be a new document
        /// and Save calls Insert
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="entity">Entity name</param>
        public void Update<T>(T entity) where T : MongoDbObject
        {
            if (entity != null)
            {
                GetCollection<T>().Save(entity);
            }
        }

        /// <summary>
        /// Deletes a row of a collection
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="id">id of the row to be deleted</param>
        public void Delete<T>(ObjectId id) where T : MongoDbObject
        {  
            IMongoQuery query = Query<T>.EQ(e => e.Id, id);
            GetCollection<T>().Remove(query);
        }

        /// <summary>
        /// Deletes a row of a collection
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="entity">Entity name</param>
        public void Delete<T>(T entity) where T : MongoDbObject
        {
            if (entity != null)
            {
                IMongoQuery query = Query<T>.EQ(e => e.Id, entity.Id);
                GetCollection<T>().Remove(query);
            }
        }

        /// <summary>
        /// Deletes a row of a collection
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        /// <param name="query">query to find the row to be deleted</param>
        public void Delete<T>(IMongoQuery query) where T : MongoDbObject
        {
            if (query != null)
            {
                GetCollection<T>().Remove(query);
            }
        }

        public void Delete<T>(Expression<Func<T, bool>> exp) where T : MongoDbObject
        {
            IMongoQuery query = Query<T>.EQ(e => e.Id, FindOne(exp).Id);
            GetCollection<T>().Remove(query);            
        }

        /// <summary>
        /// Deletes a given Table (mongo collection)
        /// </summary>
        /// <typeparam name="T">Class TypeOf</typeparam>
        public void DropCollection<T>() where T : MongoDbObject
        {
            GetCollection<T>().Drop();
        }
    }
}
