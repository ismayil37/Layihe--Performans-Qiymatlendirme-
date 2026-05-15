using System;
using System.Collections.Generic;

namespace EmployeePerformanceSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public DateTime HireDate { get; set; }

        // Davamiyyət tarixçəsi (Artıq redaktə oluna bilər)
        public List<int> AttendanceHistory { get; set; } = new List<int> { 0, 0, 0, 0, 0 };

        // Performans tarixçəsi (Aylar üzrə: Yan, Fev, Mar, Apr, May)
        public List<double> PerformanceHistory { get; set; } = new List<double> { 0, 0, 0, 0, 0 };
    }
}