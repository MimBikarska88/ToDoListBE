using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using Microsoft.VisualBasic;
using ToDoListBE.Data;
using ToDoListBE.Data.Models;
using ToDoListBE.Dto;
using ToDoListBE.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoListBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ToDoListDbContext _context;

        public TasksController(ToDoListDbContext context)
        {
            _context = context;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetToDoPendingTasks()
        {
            var tasks =  await _context.ToDoTasks.AsNoTracking()
                .Where(task => !task.IsDone && (task.DueDate >=  DateTime.UtcNow || task.DueDate == null))
                .Select(task => new TaskDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    DueDate = task.DueDate != null ? task.DueDate.Value.ToString("yyyy-MM-dd") : null,
                    IsDone = task.IsDone,
                    Description = task.Description

                }).ToListAsync();
            return Ok(tasks);
        }
        [HttpGet("overdue")]
        public async Task<IActionResult> GetToDoОverdueTasks()
        {
            var tasks = await _context.ToDoTasks.AsNoTracking()
               .Where(task => !task.IsDone && task.DueDate < DateTime.UtcNow)
               .Select(task => new TaskDto
               {
                   Id = task.Id,
                   Name = task.Name,
                   DueDate = task.DueDate != null ? task.DueDate.Value.ToString("yyyy-MM-dd") : null,
                   IsDone = task.IsDone,
                   Description = task.Description

               }).ToListAsync();
            return Ok(tasks);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(TaskDto taskDto)
        {
            try
            {
                var task = await _context.ToDoTasks.FindAsync(taskDto.Id);
                if(task == null)
                {
                    return NotFound();
                }
                DateTime? dueDate = null;

                if (!string.IsNullOrEmpty(taskDto.DueDate))
                {
                    if (!DateTime.TryParseExact(taskDto.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                    {
                        return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
                    }
                    dueDate = parsedDate;
                }

                task.Name = taskDto.Name;
                task.IsDone = taskDto.IsDone;
                task.Description = taskDto.Description;
                task.DueDate = dueDate;

                await _context.SaveChangesAsync();

                return Ok(task);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }


            throw new NotImplementedException();
        }

        //create a new task
        [HttpPost]
      
        public async Task<IActionResult> PostToDoTask(CreateTaskDto dto)
        {
            try
            {
                if(dto == null)
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                DateTime? dueDate = null;

                if (!string.IsNullOrEmpty(dto.DueDate))
                {
                    if (!DateTime.TryParseExact(dto.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                    {
                        return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
                    }
                    dueDate = parsedDate; 
                }

                var todo = new ToDoTask
                {
                    Description = dto.Description,
                    Name = dto.Name,
                    DueDate = dueDate,

                };
                await _context.ToDoTasks.AddAsync(todo);
                await _context.SaveChangesAsync();
                return Ok(todo);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
