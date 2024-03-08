using CommerceCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Application.Interface
{
    public interface IAuthService<in T, T2> 
        where T : class
        where T2: class
    {
        Task<T2> SignUpUser(T item);
    }
}
