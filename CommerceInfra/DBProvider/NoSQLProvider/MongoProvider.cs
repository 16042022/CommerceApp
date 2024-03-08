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
    public class MongoProvider: INoSQLProvider<Type, MongoClient>
    {
        private MongoClient _client;
        private IOptions<ShopDevDBSetting> _setting;

        public MongoProvider(IOptions<ShopDevDBSetting> setting)
        {
            _setting = setting;
            _client = new MongoClient(_setting.Value.ConnectionString);
        }

        public MongoClient GetConnection()
        {
            return _client;
        }

        public dynamic GetDBSchema(string DBName)
        {
            return _client.GetDatabase(_setting.Value.DatabaseName)
                .GetCollection<Type>(DBName);
        }
    }
}
