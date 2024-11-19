using System.ComponentModel.DataAnnotations;

namespace ToDoListBE.Data.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The task name is required.")]
        [MaxLength(100, ErrorMessage = "The task name cannot be longer than 100 characters.")]
        [MinLength(10, ErrorMessage = "The task name cannot be longer than 10 characters.")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "The task name is required.")]
        [MaxLength(500, ErrorMessage = "The task name cannot be longer than 500 characters.")]
        [MinLength(10, ErrorMessage = "The task name cannot be longer than 10 characters.")]
        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; } = null;

        public bool IsDone { get; set; } = false;
    }
}
