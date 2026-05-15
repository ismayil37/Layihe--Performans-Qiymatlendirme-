using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;

namespace EmployeePerformanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private static readonly string[] MonthNames = {
            "Yanvar", "Fevral", "Mart", "Aprel", "May", "İyun",
            "İyul", "Avqust", "Sentyabr", "Oktyabr", "Noyabr", "Dekabr"
        };

        private static List<Employee> _employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "Ali", LastName = "Memmedov", Records = GenerateDefaultRecords(true) },
            new Employee { Id = 2, FirstName = "Leyla", LastName = "Hesenova", Records = GenerateDefaultRecords(true) }
        };

        private static List<MonthlyRecord> GenerateDefaultRecords(bool withData = false)
        {
            var list = new List<MonthlyRecord>();
            var rnd = new Random();
            for (int i = 0; i < 12; i++)
            {
                double t = withData ? rnd.Next(60, 100) : 0;
                double q = withData ? rnd.Next(60, 100) : 0;
                double a = withData ? rnd.Next(70, 100) : 0;
                list.Add(new MonthlyRecord
                {
                    MonthName = MonthNames[i],
                    Task = t,
                    Quality = q,
                    Attendance = a,
                    FinalScore = Math.Round((t * 0.4) + (q * 0.3) + (a * 0.3), 1)
                });
            }
            return list;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_employees);

        [HttpPost]
        public IActionResult AddEmployee([FromBody] Employee newEmp)
        {
            newEmp.Id = _employees.Any() ? _employees.Max(e => e.Id) + 1 : 1;
            newEmp.Records = GenerateDefaultRecords();
            if (string.IsNullOrEmpty(newEmp.LastName)) newEmp.LastName = "-";
            _employees.Add(newEmp);
            return Ok(newEmp);
        }

        [HttpPost("{id}/update-single-month")]
        public IActionResult UpdateSingleMonth(int id, [FromBody] MonthlyUpdateModel data)
        {
            var emp = _employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) return NotFound();
            var record = emp.Records[data.MonthIndex];
            record.Task = data.Task;
            record.Quality = data.Quality;
            record.Attendance = data.Attendance;
            record.FinalScore = Math.Round((data.Task * 0.4) + (data.Quality * 0.3) + (data.Attendance * 0.3), 1);
            return Ok(emp);
        }

        [HttpGet("compare")]
        public IActionResult Compare([FromQuery] int id1, [FromQuery] int id2)
        {
            var e1 = _employees.FirstOrDefault(e => e.Id == id1);
            var e2 = _employees.FirstOrDefault(e => e.Id == id2);
            if (e1 == null || e2 == null) return NotFound();
            var e1L5 = e1.Records.Skip(7).Take(5).ToList();
            var e2L5 = e2.Records.Skip(7).Take(5).ToList();
            return Ok(new
            {
                first = new { e1.FirstName, Labels = e1L5.Select(r => r.MonthName), PerformanceHistory = e1L5.Select(r => r.FinalScore), AttendanceHistory = e1L5.Select(r => r.Attendance) },
                second = new { e2.FirstName, Labels = e2L5.Select(r => r.MonthName), PerformanceHistory = e2L5.Select(r => r.FinalScore), AttendanceHistory = e2L5.Select(r => r.Attendance) }
            });
        }

        [HttpGet("export")]
        public IActionResult Export()
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Hesabat");
                ws.Cell(1, 1).Value = "İşçi";
                for (int i = 0; i < 12; i++) ws.Cell(1, i + 2).Value = MonthNames[i];
                for (int i = 0; i < _employees.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = $"{_employees[i].FirstName} {_employees[i].LastName}";
                    for (int j = 0; j < 12; j++) ws.Cell(i + 2, j + 2).Value = _employees[i].Records[j].FinalScore;
                }
                using (var ms = new MemoryStream()) { workbook.SaveAs(ms); return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Performance_Report.xlsx"); }
            }
        }

        [HttpPost("import")]
        public IActionResult Import(IFormFile file)
        {
            if (file == null) return BadRequest();
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                using (var workbook = new XLWorkbook(ms))
                {
                    var rows = workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1);
                    foreach (var row in rows)
                    {
                        var name = row.Cell(1).GetValue<string>();
                        if (!string.IsNullOrEmpty(name)) _employees.Add(new Employee { Id = _employees.Max(e => e.Id) + 1, FirstName = name, LastName = "Excel", Records = GenerateDefaultRecords(true) });
                    }
                }
            }
            return Ok();
        }
    }

    public class Employee { public int Id { get; set; } public string FirstName { get; set; } public string LastName { get; set; } public List<MonthlyRecord> Records { get; set; } }
    public class MonthlyRecord { public string MonthName { get; set; } public double Task { get; set; } public double Quality { get; set; } public double Attendance { get; set; } public double FinalScore { get; set; } }
    public class MonthlyUpdateModel { public int MonthIndex { get; set; } public double Task { get; set; } public double Quality { get; set; } public double Attendance { get; set; } }
}