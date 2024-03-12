using CommerceCore.Application.Interface;
using CommerceCore.Application.Models;
using CommerceCore.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceInfra.DBProvider.NoSQLProvider
{
    public class MongoProvider<T, DBProvider>: INoSQLProvider<T, DBProvider>
        where T: class
        where DBProvider : MongoClient
    {
        private MongoClient _client;
        private IOptions<ShopDevDBSetting> _setting;

        public MongoProvider(IOptions<ShopDevDBSetting> setting)
        {
            _setting = setting;
            _client = new MongoClient(_setting.Value.ConnectionString);
        }

        public dynamic GetDBSchema(string DBName)
        {
            return _client.GetDatabase(_setting.Value.DatabaseName)
                .GetCollection<T>(DBName);
        }

        public DBProvider GetConnection()
        {
            return (DBProvider)_client;
        }
    }
}
