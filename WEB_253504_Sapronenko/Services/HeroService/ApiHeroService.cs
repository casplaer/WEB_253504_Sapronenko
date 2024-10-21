using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;
using WEB_253504_Sapronenko.UI.Services.Authentication;
using WEB_253504_Sapronenko.UI.Services.FileService;

namespace WEB_253504_Sapronenko.UI.Services.HeroService
{
    public class ApiHeroService : IHeroService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiHeroService> _logger;
        private readonly IFileService _fileService;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly string _pageSize = "3";
        private readonly ITokenAccessor _tokenAccessor;

        public ApiHeroService(HttpClient httpClient,
                                IConfiguration configuration,
                                ILogger<ApiHeroService> logger,
                                ITokenAccessor tokenAccessor,
                                IFileService fileService)
        {
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
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
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

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

            var requestHeaders = _httpClient.DefaultRequestHeaders;

            if (requestHeaders.Authorization == null)
            {
                return ResponseData<ListModel<DotaHero>>.Error("Authorization token is missing.");
            }

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
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile!);
                if (!string.IsNullOrEmpty(imageUrl))
                    hero.Image = imageUrl;
            }

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var url = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dotaheroes/");

            var response = await _httpClient.PostAsJsonAsync(url.ToString(), hero, _serializerOptions);

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
            if (hero.Image != null && formFile != null)
            {
                await _fileService.DeleteFileAsync(hero.Image.Split("/").Last());
                hero.Image = null;
            }
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile!);
                if (!string.IsNullOrEmpty(imageUrl))
                    hero.Image = imageUrl;
            }

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}dotaheroes/" + id.ToString());

            var response = await _httpClient.PutAsJsonAsync(new Uri(urlString.ToString()), hero, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task DeleteHeroAsync(int id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

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
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

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
