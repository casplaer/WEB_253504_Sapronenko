using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.UI.Services.HeroService;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class EditModel : PageModel
    {
        private readonly IHeroService _heroService;

        public EditModel(IHeroService heroService)
        {
            _heroService = heroService;
        }

        [BindProperty]
        public DotaHero DotaHero { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allHeroes = _heroService.GetHeroListAsync();
            var dotahero =  allHeroes.Result.Data.Items.FirstOrDefault(m => m.Id == id);
            if (dotahero == null)
            {
                return NotFound();
            }
            DotaHero = dotahero;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _heroService.UpdateHeroAsync(DotaHero.Id, DotaHero);
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
            return _heroService.GetHeroListAsync().Result.Data.Items.Any(e => e.Id == id);
        }
    }
}
