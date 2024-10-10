using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.UI.Services.HeroService;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public DotaHero DotaHero { get; set; }
        private readonly IHeroService _heroService;

        public CreateModel(IHeroService heroService)
        {
            _heroService = heroService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DotaHero dotaHero { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _heroService.CreateHeroAsync(dotaHero);

            return RedirectToPage("./Index");
        }
    }
}
