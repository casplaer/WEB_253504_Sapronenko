using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.API.Data;
using WEB_253504_Sapronenko.Domain.Entites;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class EditModel : PageModel
    {
        private readonly WEB_253504_Sapronenko.API.Data.AppDbContext _context;

        public EditModel(WEB_253504_Sapronenko.API.Data.AppDbContext context)
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

            var dotahero =  await _context.Heroes.FirstOrDefaultAsync(m => m.Id == id);
            if (dotahero == null)
            {
                return NotFound();
            }
            DotaHero = dotahero;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DotaHero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DotaHeroExists(DotaHero.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DotaHeroExists(int id)
        {
            return _context.Heroes.Any(e => e.Id == id);
        }
    }
}
