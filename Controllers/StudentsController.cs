using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oblig3dat154.Data;

namespace oblig3dat154.Controllers
{
    public class StudentsController : Controller
    {
        private readonly Dat154Context _context;

        public StudentsController(Dat154Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(s => s.Studentname.Contains(searchString));
            }

            var students = await query
                .OrderBy(s => s.Studentname)
                .ToListAsync();

            ViewBag.SearchString = searchString;
            return View(students);
        }
    }
}