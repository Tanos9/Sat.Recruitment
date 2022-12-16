using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Insert(T obj);
    }
}

