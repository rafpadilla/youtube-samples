using RepositorySample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySample.Repositories
{
    public interface ITodoRepository
    {
        Task<Todo> GetById(Guid id);
        Task<IEnumerable<Todo>> Get();
        Task CreateAndSave(Todo todo);
        Task UpdateAndSave(Todo todo);
        Task DeleteAndSave(Guid id);
    }
}
