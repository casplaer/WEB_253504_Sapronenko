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
    public class IndexModel : PageModel
    {
        private readonly WEB_253504_Sapronenko.API.Data.AppDbContext _context;

        public IndexModel(WEB_253504_Sapronenko.API.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<DotaHero> DotaHero { get;set; } = default!;

        public async Task OnGetAsync()
        {
            DotaHero = await _context.Heroes.ToListAsync();
        }
    }
}
