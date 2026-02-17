using CodeFirstDb_2026_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstDb_2026_1.Pages.Students
{
    public class DetailsModel : PageModel
    {
        #region Variables, Fields and Properties

        // This is the database context that allows us to interact with the database.
        private readonly ApplicationDbContext _context;
        // This is the Student that we will be displaying details for. It is not nullable because we will initialize it in the OnGetAsync method.
        public Student Student { get; set; } = default!;
        // This is the list of courses that are available for the student to enroll in. It is not nullable because we will initialize it in the OnGetAsync method.
        public List<SelectListItem> Courses { get; set; } = default!;
        // This is the selected course that the student will be enrolled in. We have made it a bind property so that it can be accessed in the OnPostAsync method.
        [BindProperty]
        public string SelectedCourse { get; set; } = default!;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the DetailsModel class with the specified database context.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Get and Post Methods

        /// <summary>
        /// Retrieves student details and available courses for the specified student ID.
        /// </summary>
        /// <param name="id">The student ID to retrieve.</param>
        /// <returns>A page result if the student and courses are found; otherwise, a NotFound result.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            Courses = await _context.Courses.Select(x => new SelectListItem
            {
                Value = x.CourseId.ToString(),
                Text = x.Name
            }).ToListAsync();
            var student = await _context.Students.Include("Courses").FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null || Courses == null) return NotFound();
            Student = student;
            return Page();
        }

        /// <summary>
        /// Removes the specified course from the student's course list and saves changes.
        /// </summary>
        /// <param name="id">The identifier of the course to remove.</param>
        /// <param name="StudentId">The identifier of the student.</param>
        /// <returns>A redirect to the student details page or a NotFound result if the course or student does not exist.</returns>
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

        /// <summary>
        /// Associates a course with a student and saves the changes to the database.
        /// </summary>
        /// <param name="StudentId">The identifier of the student to associate with the course.</param>
        /// <returns>A redirect to the details page if successful; NotFound if the student or course is not found.</returns>
        public async Task<IActionResult> OnPostAsync(int StudentId)
        {
            if (StudentId == 0 || SelectedCourse == null) return NotFound();
            var student = await _context.Students.Include("Courses").FirstOrDefaultAsync(m => m.StudentId == StudentId);
            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == int.Parse(SelectedCourse));
            if (student == null || course == null) return NotFound();
            student.Courses.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = StudentId });
        }

        #endregion
    }
}
