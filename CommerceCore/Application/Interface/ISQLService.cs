using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Application.Interface
{
    public interface ISQLService<T> where T : class
    {
        Task<T> Add(T obj);
        Task AddRange (IEnumerable<T> list);
        Task<IQueryable<T>> GetAll();
        Task UpdateRecord (T obj);
        Task DeleteRecord (string id);
    }
}
