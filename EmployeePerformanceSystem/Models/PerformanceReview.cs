namespace EmployeePerformanceSystem.Models
{
    public class PerformanceReview
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public double TaskCompletionScore { get; set; } // 0-100 arası
        public double QualityScore { get; set; } // Menecer rəyi (1-10 arası)
        public double AttendanceScore { get; set; } // Davamiyyət (0-100 arası)
        public double FinalPerformanceScore { get; set; } // Düsturdan çıxan yekun bal
        public DateTime ReviewDate { get; set; }
    }
}