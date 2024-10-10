using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.UI.Services.HeroService
{
    public interface IHeroService
    {
        public Task<ResponseData<ListModel<DotaHero>>> GetHeroListAsync(string? categoryNormalizedName = default, int pageNo=1);

        public Task UpdateHeroAsync(int id, DotaHero hero, IFormFile? formFile = default);

        public Task DeleteHeroAsync(int id);

        public Task<ResponseData<DotaHero>> CreateHeroAsync(DotaHero hero, IFormFile? formFile = default);
    }
}
