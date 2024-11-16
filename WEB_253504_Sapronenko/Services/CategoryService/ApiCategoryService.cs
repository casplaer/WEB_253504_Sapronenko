using System.Text;
using System.Text.Json;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;


        public ApiCategoryService(HttpClient httpClient, IConfiguration configuration) 
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public Task<ResponseData<Category>> GetHeroCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<List<Category>>> GetCategoriesAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}categories/");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return (await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_jsonSerializerOptions))!;
                }
                catch (JsonException ex)
                {
                    return ResponseData<List<Category>>.Error($"Ошибка: {ex.Message}");
                }
            }

            return ResponseData<List<Category>>.Error($"Данные не получены от сервера. Ошибка: " +
                $"{response.StatusCode.ToString()}");
        }
    }
}
