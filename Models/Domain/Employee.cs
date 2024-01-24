namespace TodoApp.Models.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Double Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; }
        public string UserId { get; set; }

    }
}
