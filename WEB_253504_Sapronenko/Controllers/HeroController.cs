using Microsoft.AspNetCore.Mvc;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;
using WEB_253504_Sapronenko.UI.Services.CategoryService;
using WEB_253504_Sapronenko.UI.Services.HeroService;

namespace WEB_253504_Sapronenko.UI.Controllers
{
    public class HeroController : Controller
    {
        private readonly IHeroService _heroService;
        private readonly ICategoryService _categoryService;

        public HeroController(IHeroService heroService, ICategoryService categoryService)
        {
            _heroService = heroService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            switch(category)
            {
                case "agility":
                    ViewBag.currentCategory = "Ловкость";
                    break;

                case "strength":
                    ViewBag.currentCategory = "Сила";
                    break;

                case "intellect":
                    ViewBag.currentCategory = "Интеллект";
                    break;

                case "unvirsal":
                    ViewBag.currentCategory = "Универсальный";
                    break;

                default:
                    ViewBag.currentCategory = "Все";
                    break;
            }

            var heroResponse =
                await _heroService.GetHeroListAsync(category, pageNo);
            if(!heroResponse.Successfull) 
                return NotFound(heroResponse.ErrorMessage);

            var categoryResponse = await _categoryService.GetCategoriesAsync();
            if (!categoryResponse.Successfull)
                return NotFound(categoryResponse.ErrorMessage);

            ViewBag.categories = categoryResponse.Data;

            var heroes = new ListModel<DotaHero> { Items = heroResponse.Data.Items, TotalPages = heroResponse.Data.TotalPages, CurrentPage = pageNo };

            return View(heroes);
        }
    }
}
