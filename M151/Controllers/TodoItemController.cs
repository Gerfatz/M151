using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TodoItemController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all <see cref="TodoItem"/>s
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetToDos()
        {
            return await _context.ToDos.ToListAsync();
        }

        /// <summary>
        /// Returns a specific <see cref="TodoItem"/> according to the Id provided
        /// </summary>
        /// <param name="id">Id of the TodoItem</param>
        /// <returns>The TodoItem with the provided Id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.ToDos.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        /// <summary>
        /// Updates the TodoItem.
        /// </summary>
        /// <param name="id">Id of the Item</param>
        /// <param name="todoItem">Updated Model</param>
        /// <returns>Http Status Code</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new <see cref="TodoItem"/>
        /// </summary>
        /// <param name="todoItem">Model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.ToDos.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        /// <summary>
        /// Deletes the <see cref="TodoItem"/> with the provided Id
        /// </summary>
        /// <param name="id">Id of the Item to delete</param>
        /// <returns></returns>
        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.ToDos.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.ToDos.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            return _context.ToDos.Any(e => e.Id == id);
        }
    }
}
