using CodeFirstDb_2026_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

//Namespce tells you what this DetailsModel is modeling... Courses.
namespace CodeFirstDb_2026_1.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        #region Variables, Fields and Properties

        //This is the database context that allows us to interact with the database.
        private readonly ApplicationDbContext _context;
        //This is the Course that we will be displaying details for. It is nullable because it may not be found in the database.
        public Course? Course { get; set; } = default!;
        //This is the list of students that are enrolled in the course. It is not nullable because we will initialize it in the OnGetAsync method.
        public List<Student> StudentList { get; set; } = default!;

        #endregion

        #region 

        //Constructor that takes in the database context and assigns it to the private variable.
        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Post and Get Methods
        
        /// <summary>
        /// Retrieves course details and an ordered list of students for the specified course.
        /// </summary>
        /// <param name="id">The identifier of the course to retrieve.</param>
        /// <returns>A page result containing course and student information, or a NotFound result if the course does not exist.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context == null || _context.Courses == null) return NotFound();
            Course = await _context.Courses.Include(s => s.Students).FirstOrDefaultAsync(m => m.CourseId == id);
            if (Course == null) return NotFound();
            //Note: Creating this list allows us to order the results by last name.
            StudentList = Course.Students.OrderBy(x => x.LastName).ToList();
            return Page();
        }

        /// <summary>
        /// Removes a student from a course and saves changes to the database.
        /// </summary>
        /// <param name="id">The ID of the student to remove.</param>
        /// <param name="CourseId">The ID of the course to redirect to after deletion.</param>
        /// <returns>A redirect to the course details page or a NotFound result if the student or course does not exist.</returns>
        public async Task<IActionResult> OnGetDeleteAsync(int? id, int CourseId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == id);
            var course = await _context.Courses.Include(x => x.Students).FirstOrDefaultAsync(x => x.CourseId == id);
            if (student == null || course == null) return NotFound();
            course.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = CourseId });
        }

        #endregion

    }
}
