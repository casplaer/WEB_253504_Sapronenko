using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.API.Data;
using WEB_253504_Sapronenko.API.Services.CategoryService;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.API.Services.HeroService
{
    public class HeroService : IHeroService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _context;
        private readonly List<Category> _categories;

        public HeroService(ICategoryService categoryService, AppDbContext context)
        {
            _categories = categoryService.GetCategoriesAsync().Result.Data;
            _context = context;
        }

        public Task<ResponseData<DotaHero>> CreateHeroAsync(DotaHero hero, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<DotaHero>> GetHeroByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<DotaHero>>> GetHeroListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Heroes.AsQueryable();
            var dataList = new ListModel<DotaHero>();

            if(categoryNormalizedName.ToLower() != "any")
            {
                query = query
                .Where(d => categoryNormalizedName == null
                ||
                d.Category.NormalizedName.Equals(categoryNormalizedName));
            }

            var count = await query.CountAsync(); //.Count();
            if (count == 0)
            {
                return ResponseData<ListModel<DotaHero>>.Success(dataList);
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<DotaHero>>.Error("No such page");

            dataList.Items = await query
            .OrderBy(d => d.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return ResponseData<ListModel<DotaHero>>.Success(dataList);

        }

        public Task UpdateHeroAsync(int id, DotaHero hero, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
