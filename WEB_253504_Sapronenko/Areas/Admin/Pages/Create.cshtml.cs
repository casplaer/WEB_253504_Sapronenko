using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.UI.Services.CategoryService;
using WEB_253504_Sapronenko.UI.Services.HeroService;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class CreateModel : PageModel
    {
        private readonly IHeroService _heroService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IHeroService heroService, ICategoryService categoryService)
        {
            _heroService = heroService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            Categories = (await _categoryService.GetCategoriesAsync()).Data!;
            return Page();  
        }

        [BindProperty]
        public DotaHero DotaHero { get; set; }
        [BindProperty]
        public IFormFile file { get; set; }
        [BindProperty]
        public List<Category> Categories { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            DotaHero.Category = (await _categoryService.GetCategoriesAsync()).Data!.FirstOrDefault(ci => ci.Id == DotaHero.CategoryId);

            await _heroService.CreateHeroAsync(DotaHero, file);

            return RedirectToPage("./Index");
        }
    }
}
