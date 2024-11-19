namespace ToDoListBE.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? DueDate { get; set; } = null;

        public bool IsDone { get; set; }
    }
}
