using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositorySample.Domain;
using RepositorySample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var todos = await _todoRepository.Get();
            return Ok(todos);
        }
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var todo = await _todoRepository.GetById(id);
            return Ok(todo);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Todo todo)
        {
            await _todoRepository.CreateAndSave(todo);
            return Created(string.Empty, todo.Id);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Todo todo)
        {
            await _todoRepository.UpdateAndSave(todo);
            return NoContent();
        }
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _todoRepository.DeleteAndSave(id);
            return NoContent();
        }
    }
}
