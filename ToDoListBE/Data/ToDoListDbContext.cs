using Microsoft.EntityFrameworkCore;
using ToDoListBE.Data.Models;

namespace ToDoListBE.Data
{
    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options) { }

        public DbSet<ToDoTask> ToDoTasks { get; set; }
    }
}
