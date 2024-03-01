using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceUtils.Helpers
{
    public interface IServerHealthCheck
    {
        int GetConnectionNumber(MongoClient client);
        void IsOverloadServer(MongoClient client);
    }
}
