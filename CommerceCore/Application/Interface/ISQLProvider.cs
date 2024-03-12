using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Application.Interface
{
    public interface ISQLProvider<out DBProvider> : IDisposable 
        where DBProvider : class
    {
        DBProvider GetConnection();
    }
}
