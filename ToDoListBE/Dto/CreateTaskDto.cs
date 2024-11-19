namespace ToDoListBE.Dto
{
    public class CreateTaskDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string DueDate { get; set; } = string.Empty;
    }
}
