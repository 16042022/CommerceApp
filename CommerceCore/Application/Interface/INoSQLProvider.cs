﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Application.Interface
{
    public interface INoSQLProvider<in Type, out DBProvider>
    {
        DBProvider GetConnection();

        dynamic GetDBSchema(string DBName);
    }
}
