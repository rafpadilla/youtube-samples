using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLiteSample.Data;
using SQLiteSample.Domain;
using System.Threading.Tasks;

namespace SQLiteSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var todos = await _todoContext.Todos.ToListAsync();
            return Ok(todos);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Todo todo)
        {
            _todoContext.Todos.Add(todo);
            await _todoContext.SaveChangesAsync();
            return Created(string.Empty, todo.Id);
        }
    }
}
