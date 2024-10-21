using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using WEB_253504_Sapronenko.UI.HelperClasses;
using WEB_253504_Sapronenko.UI.Models;
using WEB_253504_Sapronenko.UI.Services.FileService;
using WEB_253504_Sapronenko.UI.Services.Authentication;

namespace WEB_253504_Sapronenko.UI.Authorization
{
    public class KeycloakAuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;
        private readonly ITokenAccessor _tokenAccessor;
        KeycloakData _keycloakData;

        public KeycloakAuthService(HttpClient httpClient,
        IOptions<KeycloakData> options,
        IFileService fileService,
        ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
            _keycloakData = options.Value;
        }


        public async Task<(bool Result, string ErrorMessage)> RegisterUserAsync(
                    string email, 
                    string password, 
                    IFormFile? avatar)
        {
            {
                try
                {
                    await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
                var avatarUrl = "https://localhost:7002/Images/default-profile-image.png";
                if (avatar != null)
                {
                    var result = await _fileService.SaveFileAsync(avatar);
                    if (result != null) avatarUrl = result;
                }
                var newUser = new CreateUserModel();
                newUser.Attributes.Add("avatar", avatarUrl);
                newUser.Email = email;
                newUser.Username = email;
                newUser.Credentials.Add(new UserCredentials { Value = password });
                var requestUri =
                    $"{_keycloakData.Host}/admin/realms/{_keycloakData.Realm}/users";
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var userData = JsonSerializer.Serialize(newUser, serializerOptions);
                HttpContent content = new StringContent(userData, Encoding.UTF8,
                "application/json");
                var response = await _httpClient.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode) return (true, String.Empty);
                return (false, response.StatusCode.ToString());
            }

        }
    }
}
