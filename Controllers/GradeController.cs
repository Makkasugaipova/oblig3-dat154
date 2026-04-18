using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oblig3dat154.Data;
using oblig3dat154.Models;

namespace oblig3dat154.Controllers
{
    public class GradesController : Controller
    {
        private readonly Dat154Context _context;

        public GradesController(Dat154Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string selectedGrade)
        {
            var validGrades = new List<string> { "A", "B", "C", "D", "E", "F" };
            ViewBag.ValidGrades = validGrades;
            ViewBag.SelectedGrade = selectedGrade;

            var result = new List<GradeFilterViewModel>();

            if (!string.IsNullOrWhiteSpace(selectedGrade))
            {
                int limit = validGrades.IndexOf(selectedGrade);

                if (limit >= 0)
                {
                    var allowedGrades = validGrades.Take(limit + 1).ToList();

                    result = await _context.Grades
                        .Where(g => allowedGrades.Contains(g.Grade1))
                        .Select(g => new GradeFilterViewModel
                        {
                            Studentname = g.Student.Studentname,
                            Coursename = g.CoursecodeNavigation.Coursename,
                            Coursecode = g.Coursecode,
                            Grade = g.Grade1
                        })
                        .OrderBy(g => g.Grade)
                        .ThenBy(g => g.Studentname)
                        .ToListAsync();
                }
            }

            return View(result);
        }

        public async Task<IActionResult> Failed()
        {
            var failedStudents = await _context.Grades
                .Where(g => g.Grade1 == "F")
                .Select(g => new FailedStudentViewModel
                {
                    Studentname = g.Student.Studentname,
                    Coursename = g.CoursecodeNavigation.Coursename,
                    Coursecode = g.Coursecode,
                    Grade = g.Grade1
                })
                .OrderBy(x => x.Studentname)
                .ToListAsync();

            return View(failedStudents);
        }
    }
}