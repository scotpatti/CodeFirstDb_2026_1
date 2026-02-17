using CodeFirstDb_2026_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstDb_2026_1.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Course? Course { get; set; } = default!;
        public List<Student> StudentList { get; set; } = default!;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context == null || _context.Courses == null) return NotFound();
            Course = await _context.Courses.Include(s => s.Students).FirstOrDefaultAsync(m => m.CourseId == id);
            if (Course == null) return NotFound();
            //Note: Creating this list allows us to order the results by last name.
            StudentList = Course.Students.OrderBy(x => x.LastName).ToList();
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? id, int CourseId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == id);
            var course = await _context.Courses.Include(x => x.Students).FirstOrDefaultAsync(x => x.CourseId == id);
            if (student == null || course == null) return NotFound();
            course.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = CourseId });
        }


    }
}
