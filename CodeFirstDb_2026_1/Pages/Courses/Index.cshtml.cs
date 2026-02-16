using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CodeFirstDb_2026_1.Data;

namespace CodeFirstDb_2026_1.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly CodeFirstDb_2026_1.Data.ApplicationDbContext _context;

        public IndexModel(CodeFirstDb_2026_1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Course = await _context.Courses.ToListAsync();
        }
    }
}
