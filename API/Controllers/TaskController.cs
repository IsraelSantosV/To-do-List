using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Workspace.API.Data;
using Workspace.API.Models;

namespace Workspace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly DataContext _context;

        public TaskController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return _context.Tasks;
        }

        [HttpGet("{id}")]
        public Task Get(int id)
        {
            return _context.Tasks.FirstOrDefault(myTask => myTask.Id == id);
        }

        [HttpPost]
        public IActionResult Post(Task task)
        {
            if(task == null)
            {
                return BadRequest();
            }

            if (_context.Tasks.Any(contextTask => contextTask.Id == task.Id))
            {
                return BadRequest(new ConflictResult());
            }

            Task addedTask = _context.Tasks.Add(task).Entity;
            _context.SaveChanges();
            return Accepted(addedTask);
        }

        [HttpPut]
        public IActionResult Put(Task task)
        {
            if(task == null)
            {
                return BadRequest();
            }

            _context.Tasks.Update(task);
            _context.SaveChanges();
            return Accepted();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Task targetTask = _context.Tasks.FirstOrDefault(task => task.Id == id);
            if (targetTask == null)
            {
                return BadRequest();
            }

            _context.Tasks.Remove(targetTask);
            _context.SaveChanges();
            return Accepted();
        }
    }
}
