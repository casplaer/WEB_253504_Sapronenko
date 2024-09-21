using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.API.Services.HeroService
{
    public interface IHeroService
    {
        public Task<ResponseData<ListModel<DotaHero>>> GetHeroListAsync(string? categoryNormalizedName, int pageNo=1, int pageSize=3);

        public Task<ResponseData<DotaHero>> GetHeroByIdAsync(int id);

        public Task UpdateHeroAsync(int id, DotaHero hero, IFormFile? formFile);

        public Task DeleteProductAsync(int id);

        public Task<ResponseData<DotaHero>> CreateHeroAsync(DotaHero hero, IFormFile? formFile);
    }
}
