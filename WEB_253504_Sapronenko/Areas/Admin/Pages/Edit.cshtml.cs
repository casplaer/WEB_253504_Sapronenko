using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.UI.Services.CategoryService;
using WEB_253504_Sapronenko.UI.Services.HeroService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class EditModel : PageModel
    {
        private readonly IHeroService _heroService;
        private readonly ICategoryService _categoryService;

        public EditModel(IHeroService heroService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _heroService = heroService;
        }

        [BindProperty]
        public DotaHero DotaHero { get; set; } = default!;
        [BindProperty]
        public IFormFile? file { get; set; } = default;
        [BindProperty]
        public List<Category> Categories { get; set; } = default!;

        private string oldPath;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Categories = (await _categoryService.GetCategoriesAsync()).Data!;

            var dotahero =  (await _heroService.GetHeroByIdAsync(id ?? 0)).Data;
            oldPath = dotahero.Image;
            
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
                var cat = _categoryService.GetCategoriesAsync().Result.Data!.FirstOrDefault(c => c.Id == DotaHero.CategoryId);
                DotaHero.Category = cat;
                await _heroService.UpdateHeroAsync(DotaHero.Id, DotaHero, file);
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
