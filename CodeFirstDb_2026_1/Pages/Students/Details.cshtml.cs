using CodeFirstDb_2026_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstDb_2026_1.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly CodeFirstDb_2026_1.Data.ApplicationDbContext _context;

        public DetailsModel(CodeFirstDb_2026_1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Student Student { get; set; } = default!;
        
        public List<SelectListItem> Courses { get; set; } = default!;

        [BindProperty]
        public string SelectedCourse { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Courses = await _context.Courses.Select(x => new SelectListItem
            {
                Value = x.CourseId.ToString(),
                Text = x.Name,
                Selected = false
            }).ToListAsync();
            var student = await _context.Students.Include("Courses").FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }
            else
            {
                Student = student;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id, int StudentId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
            var student = await _context.Students.Include("Courses").FirstOrDefaultAsync(m => m.StudentId == StudentId);
            if (course == null || student == null)
            {
                return NotFound();
            }
            student.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = StudentId });
        }

        public async Task<IActionResult> OnPostAsync(int StudentId)
        {
            if (StudentId == 0 || SelectedCourse == null)
            {
                               return NotFound();
            }
            var student = await _context.Students.Include("Courses").FirstOrDefaultAsync(m => m.StudentId == StudentId);
            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == int.Parse(SelectedCourse));
            if (student == null || course == null)
            {
                return NotFound();
            }
            student.Courses.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = StudentId });
        }
    }
}
