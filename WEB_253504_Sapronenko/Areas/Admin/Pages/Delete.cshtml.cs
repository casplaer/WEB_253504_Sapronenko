using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.UI.Services.FileService;
using WEB_253504_Sapronenko.UI.Services.HeroService;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IHeroService _heroService;
        private readonly IFileService _fileService;
        public DeleteModel(IHeroService heroService, IFileService fileService)
        {
            _heroService = heroService;
            _fileService = fileService;
        }

        [BindProperty]
        public DotaHero DotaHero { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dotahero = (await _heroService.GetHeroByIdAsync(id ?? 0)).Data;

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

            var dotahero = (await _heroService.GetHeroByIdAsync(id ?? 0)).Data;

            if (dotahero != null)
            {
                DotaHero = dotahero;
                await _fileService.DeleteFileAsync(dotahero.Image!);
                await _heroService.DeleteHeroAsync(id ?? 0);
            }

            return RedirectToPage("./Index");
        }
    }
}
