using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaningeServer.Data;
using HaningeServer.Models;

namespace HaningeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null) return Unauthorized("Token saknar användar-ID.");

            int userId = int.Parse(userIdClaim);
            var todos = await _context.Todos.Where(t => t.UserId == userId).ToListAsync();
            return Ok(todos);
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] Todo todo)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null) return Unauthorized("Token saknar användar-ID.");

            todo.UserId = int.Parse(userIdClaim);
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodos), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] Todo updatedTodo)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null) return Unauthorized("Token saknar användar-ID.");

            int userId = int.Parse(userIdClaim);
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todo == null) return NotFound();

            todo.Title = updatedTodo.Title;
            todo.IsDone = updatedTodo.IsDone;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null) return Unauthorized("Token saknar användar-ID.");

            int userId = int.Parse(userIdClaim);
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todo == null) return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
