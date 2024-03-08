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
    public class MongoShopService : ISQLService<ShopExample>
    {
        private INoSQLProvider<ShopExample, MongoClient> SQLProvider;

        public MongoShopService(INoSQLProvider<ShopExample, MongoClient> sQLProvider)
        {
            SQLProvider = sQLProvider;
        }

        public async Task<ShopExample> Add(ShopExample obj)
        {
            try
            {
                var _cursor = (IMongoCollection<ShopExample>)SQLProvider.GetDBSchema("Shop");
                await _cursor.InsertOneAsync(obj); return obj;
            }
            catch (Exception ex)
            {
                throw new AggregateException(ex);
            }
        }

        public async Task AddRange(IEnumerable<ShopExample> list)
        {
            var _cursor = (IMongoCollection<ShopExample>)SQLProvider.GetDBSchema("Shop");
            await _cursor.InsertManyAsync(list);
        }

        public Task DeleteRecord(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<ShopExample>> GetAll()
        {
            var _cursor = (IMongoCollection<ShopExample>)SQLProvider.GetDBSchema("Shop");
            return await Task.Run(() => _cursor.AsQueryable());
        }

        public Task UpdateRecord(ShopExample obj)
        {
            throw new NotImplementedException();
        }
    }
}
