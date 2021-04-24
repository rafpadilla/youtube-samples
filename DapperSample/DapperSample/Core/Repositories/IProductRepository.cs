using DapperSample.Core.Entities;
using System.Threading.Tasks;

namespace DapperSample.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetById(int id);
        Task<int> Create(Product product);
        Task<int> Update(Product product);
        Task<int> Delete(int id);
    }
}
