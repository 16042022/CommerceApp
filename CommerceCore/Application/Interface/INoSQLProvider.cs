using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Application.Interface
{
    public interface INoSQLProvider<out DBProvider>
    {
        DBProvider GetConnection();
    }
}
