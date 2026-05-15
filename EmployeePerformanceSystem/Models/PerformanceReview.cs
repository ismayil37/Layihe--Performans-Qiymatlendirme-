namespace EmployeePerformanceSystem.Models
{
    public class PerformanceReview
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public double TaskCompletionScore { get; set; } 
        public double QualityScore { get; set; } 
        public double AttendanceScore { get; set; } 
        public double FinalPerformanceScore { get; set; } 
        public DateTime ReviewDate { get; set; }
    }
}
