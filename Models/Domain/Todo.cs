namespace TodoApp.Models.Domain
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Task { get; set; }
        public string UserId { get; set; }

    }
}
