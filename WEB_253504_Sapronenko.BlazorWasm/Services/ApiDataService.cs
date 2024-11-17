using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text;
using System.Net.Http.Json;
using WEB_253504_Sapronenko.Domain.Entites;
using WEB_253504_Sapronenko.Domain.Models;
using System;
using Microsoft.Extensions.Logging;

namespace WEB_253504_Sapronenko.BlazorWasm.Services
{
    public class ApiDataService : IDataService
    {
        public List<Category> Categories { get; set; }
        public List<DotaHero> Heroes { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public Category? SelectedCategory { get; set; }

        public event Action DataLoaded;

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiDataService> _logger;
        private readonly IAccessTokenProvider _tokenProvider;

        public ApiDataService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiDataService> logger, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _tokenProvider = tokenProvider;
        }

        public async Task GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}categories");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
                Categories = data!.Data!;
                ErrorMessage = data.ErrorMessage ?? "";
            }
        }

        public async Task GetProductListAsync(int pageNo = 1)
        {
            _logger.LogWarning($"--------------------Current page: {pageNo}");
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (!tokenRequest.TryGetToken(out var token))
            {
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
            var pageSize = _configuration.GetValue<int>("ItemsPerPage");
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}dotaheroes/");

            if (SelectedCategory is not null)
            {
                urlString.Append($"{SelectedCategory.NormalizedName}-dotaheroes/");
            }
            else
            {
                urlString.Append("any-dotaheroes/");
            }
            
            urlString.Append($"page{pageNo}");

            if (pageNo == 0)
                urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dotaheroes/");

            _logger.LogInformation("Request URL: {Url}", urlString.ToString());

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                _logger.LogWarning(urlString.ToString());
                _logger.LogWarning(await response.Content.ReadAsStringAsync());

                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("JSON Response: {JsonResponse}", jsonResponse);

                var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<DotaHero>>>();
                Heroes = data!.Data!.Items;
                Success = data.Successfull;
                TotalPages = data.Data.TotalPages;
                CurrentPage = data.Data.CurrentPage;
                ErrorMessage = data.ErrorMessage ?? "";
                DataLoaded?.Invoke();
            }
        }
    }
}
