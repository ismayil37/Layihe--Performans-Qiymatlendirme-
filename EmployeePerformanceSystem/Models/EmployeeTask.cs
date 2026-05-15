namespace EmployeePerformanceSystem.Models
{
    public class EmployeeTask
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } 
        public string Title { get; set; }
        public DateTime DueDate { get; set; } 
        public DateTime? CompletedDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
