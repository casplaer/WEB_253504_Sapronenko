using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_Sapronenko.API.Data;
using WEB_253504_Sapronenko.Domain.Entites;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class CreateModel : PageModel
    {
        private readonly WEB_253504_Sapronenko.API.Data.AppDbContext _context;

        public CreateModel(WEB_253504_Sapronenko.API.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DotaHero DotaHero { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Heroes.Add(DotaHero);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
