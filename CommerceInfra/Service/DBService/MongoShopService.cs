using CommerceCore.Application.Interface;
using CommerceCore.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommerceInfra.Service.DBService
{
    public class MongoShopService<T> : ISQLService<T> where T: class
    {
        private readonly INoSQLProvider<T, MongoClient> SQLProvider;

        public MongoShopService(INoSQLProvider<T, MongoClient> sQLProvider)
        {
            SQLProvider = sQLProvider;
        }

        public async Task<T> Add(T obj)
        {
            try
            {
                var _cursor = (IMongoCollection<T>)SQLProvider.GetDBSchema(typeof(T).Name);
                await _cursor.InsertOneAsync(obj); return obj;
            }
            catch (Exception ex)
            {
                throw new AggregateException(ex);
            }
        }

        public async Task AddRange(IEnumerable<T> list)
        {
            var _cursor = (IMongoCollection<T>)SQLProvider.GetDBSchema(typeof(T).Name);
            await _cursor.InsertManyAsync(list);
        }

        public Task DeleteRecord(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRecord(T obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<T>> GetAll()
        {
            var _cursor = (IMongoCollection<T>)SQLProvider.GetDBSchema(typeof(T).Name);
            return await Task.Run(() => _cursor.AsQueryable());
        }
    }
}
