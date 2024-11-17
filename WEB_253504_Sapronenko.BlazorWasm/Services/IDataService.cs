using WEB_253504_Sapronenko.Domain.Entites;

namespace WEB_253504_Sapronenko.BlazorWasm.Services
{
    public interface IDataService
    {
        event Action DataLoaded;
        // Список категорий объектов
        List<Category> Categories { get; set; }
        //Список объектов
        List<DotaHero> Heroes { get; set; }
        // Признак успешного ответа на запрос к Api
        bool Success { get; set; }
        // Сообщение об ошибке
        string ErrorMessage { get; set; }
        // Количество страниц списка
        int TotalPages { get; set; }
        // Номер текущей страницы
        int CurrentPage { get; set; }
        // Фильтр по категории
        Category? SelectedCategory { get; set; }

        public Task GetProductListAsync(int pageNo = 1);

        public Task GetCategoryListAsync();
    }
}
