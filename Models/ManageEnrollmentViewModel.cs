using Microsoft.AspNetCore.Mvc.Rendering;

namespace oblig3dat154.Models
{
    public class ManageEnrollmentViewModel
    {
        public int Studentid { get; set; }
        public string Coursecode { get; set; } = null!;
        public string Grade1 { get; set; } = null!;

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Courses { get; set; } = new();
        public List<SelectListItem> Grades { get; set; } = new();
    }
}