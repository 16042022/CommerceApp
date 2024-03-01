using CommerceCore.Application.Interface;
using CommerceCore.Application.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceInfra.DBProvider.NoSQLProvider
{
    public class MongoProvider : INoSQLProvider<MongoClient>
    {
        private MongoClient _client;
        private IOptions<ShopDevDBSetting> _setting;

        private MongoProvider(IOptions<ShopDevDBSetting> setting)
        {
            _setting = setting;
            _client = new MongoClient(setting.Value.ConnectionString);
        }

        public MongoClient GetConnection()
        {
            if (_client == null)
            {
                var connect = new MongoProvider(_setting)._client;
                _client = connect;
            }
            return _client;
        }
    }
}
