using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using oblig3dat154.Data;
using oblig3dat154.Models;

namespace oblig3dat154.Controllers
{
    public class CoursesController : Controller
    {
        private readonly Dat154Context _context;

        public CoursesController(Dat154Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string selectedCourseCode)
        {
            ViewBag.Courses = await _context.Courses
                .OrderBy(c => c.Coursename)
                .ToListAsync();

            var result = new List<CourseGradesViewModel>();

            if (!string.IsNullOrWhiteSpace(selectedCourseCode))
            {
                result = await _context.Grades
                    .Where(g => g.Coursecode == selectedCourseCode)
                    .Select(g => new CourseGradesViewModel
                    {
                        Coursecode = g.Coursecode,
                        Coursename = g.CoursecodeNavigation.Coursename,
                        Studentname = g.Student.Studentname,
                        Grade = g.Grade1
                    })
                    .OrderBy(x => x.Studentname)
                    .ToListAsync();
            }

            ViewBag.SelectedCourseCode = selectedCourseCode;
            return View(result);
        }

        public async Task<IActionResult> Manage()
        {
            var vm = new ManageEnrollmentViewModel
            {
                Students = await _context.Students
                    .OrderBy(s => s.Studentname)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Studentname + " (ID: " + s.Id + ")"
                    })
                    .ToListAsync(),

                Courses = await _context.Courses
                    .OrderBy(c => c.Coursename)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Coursecode,
                        Text = c.Coursename + " (" + c.Coursecode + ")"
                    })
                    .ToListAsync(),

                Grades = new List<SelectListItem>
                {
                    new SelectListItem { Value = "A", Text = "A" },
                    new SelectListItem { Value = "B", Text = "B" },
                    new SelectListItem { Value = "C", Text = "C" },
                    new SelectListItem { Value = "D", Text = "D" },
                    new SelectListItem { Value = "E", Text = "E" },
                    new SelectListItem { Value = "F", Text = "F" }
                }
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEnrollment(ManageEnrollmentViewModel vm)
        {
            bool exists = await _context.Grades.AnyAsync(g =>
                g.Studentid == vm.Studentid && g.Coursecode == vm.Coursecode);

            if (!exists)
            {
                var grade = new Grade
                {
                    Studentid = vm.Studentid,
                    Coursecode = vm.Coursecode,
                    Grade1 = vm.Grade1
                };

                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveEnrollment(int studentid, string coursecode)
        {
            var existing = await _context.Grades.FindAsync(coursecode, studentid);

            if (existing != null)
            {
                _context.Grades.Remove(existing);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}