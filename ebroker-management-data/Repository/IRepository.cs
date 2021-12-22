using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EBroker.Management.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        T Add(T t);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
    }
}
