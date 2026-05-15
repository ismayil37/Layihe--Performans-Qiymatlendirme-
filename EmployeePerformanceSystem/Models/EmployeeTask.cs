namespace EmployeePerformanceSystem.Models
{
    public class EmployeeTask
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } // Hansı işçiyə aiddir?
        public string Title { get; set; }
        public DateTime DueDate { get; set; } // Bitməli olduğu vaxt
        public DateTime? CompletedDate { get; set; } // Faktiki bitmə vaxtı
        public bool IsCompleted { get; set; } // Tamamlanıb?
    }
}
