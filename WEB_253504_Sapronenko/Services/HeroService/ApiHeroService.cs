using System.Text;
using System.Text.Json;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;

namespace WEB_253504_Sapronenko.UI.Services.HeroService
{
    public class ApiHeroService : IHeroService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiHeroService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly string _pageSize = "3";

        public ApiHeroService(HttpClient httpClient,
                                IConfiguration configuration,
                                ILogger<ApiHeroService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
        }

        public async Task<ResponseData<ListModel<DotaHero>>> GetHeroListAsync(string? categoryNormalizedName = default, int pageNo = 1)
        {
            var url = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dotaheroes/");

            if (categoryNormalizedName != null)
            {
                url.Append(categoryNormalizedName + "-dotaheroes/");
            }
            else
            {
                url.Append("any-dotaheroes/");
            }

            url.Append($"page{pageNo}");

            if (!_pageSize.Equals("3"))
            {
                url.Append(QueryString.Create("pageSize", _pageSize));
            }

            var response = await _httpClient.GetAsync(new Uri(url.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<ListModel<DotaHero>>>
                                                            (_serializerOptions);
            }

            return ResponseData<ListModel<DotaHero>>.Error($"Ошибка:{ response.StatusCode.ToString() }");
        }

        public async Task<ResponseData<DotaHero>> CreateHeroAsync(DotaHero hero, IFormFile? formFile = default)
        {
            var url = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}");

            var jsonHero = JsonSerializer.Serialize(hero);

            var content = new StringContent(jsonHero, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url.ToString(), content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<ResponseData<DotaHero>>();
                return responseData!;
            }
            else
            {
                return ResponseData<DotaHero>.Error($"Ошибка:{response.StatusCode.ToString()}");
            }
        }

        public Task DeleteHeroAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task UpdateHeroAsync(int id, DotaHero hero, IFormFile? formFile = default)
        {
            throw new NotImplementedException();
        }
    }
}
