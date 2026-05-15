namespace EmployeePerformanceSystem.Models
{
    public class MonthlyUpdateModel
    {
        public int MonthIndex { get; set; } // 0=Yan, 1=Fev, ... 4=May
        public int Attendance { get; set; }
        public double Performance { get; set; }
    }
}