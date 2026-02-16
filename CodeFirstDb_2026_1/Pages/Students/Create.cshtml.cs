using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CodeFirstDb_2026_1.Data;

namespace CodeFirstDb_2026_1.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly CodeFirstDb_2026_1.Data.ApplicationDbContext _context;

        [BindProperty]
        public Student Student { get; set; } = default!;

        public CreateModel(CodeFirstDb_2026_1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
