using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;
using WEB_253504_Sapronenko.UI.Services.HeroService;

namespace WEB_253504_Sapronenko.UI.Areas.Admin.Views
{
    public class IndexModel : PageModel
    {
        private readonly IHeroService _heroService;

        public IndexModel(IHeroService heroService)
        {
            _heroService = heroService;
        }

        public IList<DotaHero> DotaHero { get;set; } = default!;

        public async Task OnGetAsync()
        {
            DotaHero = (await _heroService.GetHeroListAsync(null, 0)).Data!.Items;
        }
    }
}
