using System.Net.Http.Headers;
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

            if(pageNo == 0)
                url = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dotaheroes/");

            var response = await _httpClient.GetAsync(new Uri(url.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<ListModel<DotaHero>>>
                                                            (_serializerOptions);
            }

            return ResponseData<ListModel<DotaHero>>.Error($"Ошибка:{response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<DotaHero>> CreateHeroAsync(DotaHero hero, IFormFile? formFile = default)
        {

            var url = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dotaheroes/");

            using var content = new MultipartFormDataContent();

            var heroJson = JsonSerializer.Serialize(hero);
            content.Add(new StringContent(heroJson, Encoding.UTF8, "application/json"), "DotaHero");

            if (formFile != null)
            {
                var fileStream = formFile.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                content.Add(fileContent, "Image", formFile.FileName);
            }

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
        public async Task UpdateHeroAsync(int id, DotaHero hero, IFormFile? formFile = default)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}dotaheroes/" + id.ToString());

            using var content = new MultipartFormDataContent();

            var heroJson = JsonSerializer.Serialize(hero);

            var idJson = JsonSerializer.Serialize(id);

            content.Add(new StringContent(idJson, Encoding.UTF8, "application/json"), "idJson");

            content.Add(new StringContent(heroJson, Encoding.UTF8, "application/json"), "jsonDotaHero");

            if (formFile != null)
            {
                var fileStream = formFile.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                content.Add(fileContent, "Image", formFile.FileName);
            }

            var response = await _httpClient.PutAsync(new Uri(urlString.ToString()), content);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task DeleteHeroAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}dotaheroes/" + id.ToString());

            var response = await _httpClient.DeleteAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task<ResponseData<DotaHero>> GetHeroByIdAsync(int id)
        {
            var url = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dotaheroes/");

            url.Append(id);

            var response = await _httpClient.GetAsync(new Uri(url.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<DotaHero>>
                                                            (_serializerOptions);
            }
            else
            {
                return ResponseData<DotaHero>.Error($"Ошибка:{response.StatusCode.ToString()}");
            }
        }
    }
}
