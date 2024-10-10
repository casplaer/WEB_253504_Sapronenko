using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.UI.Services.HeroService;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class DeleteModel : PageModel
    {
        private readonly IHeroService _heroService;

        public DeleteModel(IHeroService heroService)
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
            var dotahero = allHeroes.Result.Data.Items.FirstOrDefault(m => m.Id == id);

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

            var allHeroes = _heroService.GetHeroListAsync();
            var dotahero = allHeroes.Result.Data.Items.Find(dh=>dh.Id == id);
            if (dotahero != null)
            {
                DotaHero = dotahero;
                _heroService.DeleteHeroAsync(DotaHero.Id);
            }

            return RedirectToPage("./Index");
        }
    }
}
