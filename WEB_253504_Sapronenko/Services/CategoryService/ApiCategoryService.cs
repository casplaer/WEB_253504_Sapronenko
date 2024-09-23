using System.Text;
using System.Text.Json;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;
using WEB_253504_Sapronenko.UI.Models;

namespace WEB_253504_Sapronenko.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiCategoryService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<ResponseData<List<Category>>> GetCategoriesAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}categories/");

            var response = await _httpClient.GetAsync(urlString.ToString());

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_jsonSerializerOptions);
            }

            return ResponseData<List<Category>>.Error($"Данные не получены от сервера. Ошибка: " +
                $"{response.StatusCode.ToString()}");
        }
    }
}
