using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoriesAsync()
        {
            var categories = new List<Category>
{
                    new Category {Id=1, Name="Ловкость",
                    NormalizedName="agility"},
                    new Category {Id=2, Name="Сила",
                    NormalizedName="strength"},
                    new Category {Id=3, Name="Интеллект", NormalizedName="intellect"},
                    new Category {Id=4, Name="Универсальный",
                    NormalizedName="universal"},
};
            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }
    }
}
