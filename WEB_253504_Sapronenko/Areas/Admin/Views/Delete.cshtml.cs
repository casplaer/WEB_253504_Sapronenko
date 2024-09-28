using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.API.Data;
using WEB_253504_Sapronenko.Domain.Entites;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class DeleteModel : PageModel
    {
        private readonly WEB_253504_Sapronenko.API.Data.AppDbContext _context;

        public DeleteModel(WEB_253504_Sapronenko.API.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DotaHero DotaHero { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dotahero = await _context.Heroes.FirstOrDefaultAsync(m => m.Id == id);

            if (dotahero == null)
            {
                return NotFound();
            }
            else
            {
                DotaHero = dotahero;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dotahero = await _context.Heroes.FindAsync(id);
            if (dotahero != null)
            {
                DotaHero = dotahero;
                _context.Heroes.Remove(DotaHero);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
