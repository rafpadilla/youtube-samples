using Microsoft.EntityFrameworkCore;
using RepositorySample.Data;
using RepositorySample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySample.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _todoContext;

        public TodoRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }
        public async Task CreateAndSave(Todo todo)
        {
            _todoContext.Todos.Add(todo);
            await _todoContext.SaveChangesAsync();
        }

        public async Task DeleteAndSave(Guid id)
        {
            var todo = new Todo() { Id = id };
            _todoContext.Todos.Remove(todo);
            await _todoContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Todo>> Get()
        {
            return await _todoContext.Todos.ToListAsync();
        }

        public Task<Todo> GetById(Guid id)
        {
            return _todoContext.Todos.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        public async Task UpdateAndSave(Todo todo)
        {
            _todoContext.Todos.Update(todo);
            await _todoContext.SaveChangesAsync();
        }
    }
}
