using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperSample.Core.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
    }
}
