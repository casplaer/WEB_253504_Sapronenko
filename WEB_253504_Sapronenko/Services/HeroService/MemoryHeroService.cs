using Microsoft.AspNetCore.Mvc;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;
using WEB_253504_Sapronenko.UI.Services.CategoryService;

namespace WEB_253504_Sapronenko.UI.Services.HeroService
{
    public class MemoryHeroService : IHeroService
    {
        List<DotaHero> _heroes;
        List<Category> _categories;
        private IConfiguration _configuration;

        public MemoryHeroService(
                [FromServices] IConfiguration config,
                ICategoryService categoryService,
                int pageNo = 1)
        {
            _categories = categoryService.GetCategoriesAsync()
                                            .Result
                                            .Data;

            _configuration = config;
            SetupData();
        }

        private void SetupData() 
        {
            _heroes = new List<DotaHero>
            {
                new DotaHero { Id = 1,
                                    Name = "Juggernaut",
                                    Description = "Воин с острова масок.",
                                    BaseDamage = 49, Image = "images/Juggernaut.jpg",
                                    Category=
                                    _categories.Find(c=>c.NormalizedName.Equals("agility"))},
                new DotaHero { Id = 2,
                                    Name = "Pudge",
                                    Description = "Я как шаверма, если не повезло, будешь страдать...",
                                    BaseDamage = 68, Image = "images/Pudge.jpg",
                                    Category=
                                    _categories.Find(c=>c.NormalizedName.Equals("strength"))},
                new DotaHero {Id = 3,
                                    Name = "Keeper of the Light",
                                    Description = "На бледном жеребце скачет, мчит на защиту света искра бессчётных солнц, Эзалор.",
                                    BaseDamage = 43, Image = "images/KOTL.jpg",
                                    Category=
                                    _categories.Find(c=>c.NormalizedName.Equals("intellect"))},
                new DotaHero {Id = 4,
                                    Name = "Invoker",
                                    Description = "Cреди всех волшебников было всего одно, но гениальное и владеющее огромной памятью исключение, и имя ему — Invoker.",
                                    BaseDamage = 39, Image = "images/Invoker.jpg",
                                    Category=
                                    _categories.Find(c=>c.NormalizedName.Equals("universal"))},
            };
        }
        public Task<ResponseData<DotaHero>> CreateHeroAsync(DotaHero hero, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteHeroAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<DotaHero>>> GetHeroListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var pageSize = _configuration.GetValue<int>("ItemsPerPage");

            IEnumerable<DotaHero> filteredHeroes = _heroes;

            if (!string.IsNullOrWhiteSpace(categoryNormalizedName))
            {
                filteredHeroes = _heroes
                    .Where(h => h.Category != null && h.Category.NormalizedName.Equals(categoryNormalizedName, StringComparison.OrdinalIgnoreCase));
            }

            double totalHeroes = filteredHeroes.Count();
            filteredHeroes = filteredHeroes
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize);

            var result = new ListModel<DotaHero>
            {
                Items = filteredHeroes.ToList(),
                TotalPages = (int)Math.Ceiling(totalHeroes / 3)
            };

            return await Task.FromResult (new ResponseData<ListModel<DotaHero>>
            {
                Data = result,
            });
        }

        public Task UpdateHeroAsync(int id, DotaHero hero, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

    }
}
