namespace WEB_253504_Sapronenko.UI.Services.Authentication
{
    public interface ITokenAccessor
    {
        Task<string> GetAccessTokenAsync();
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);
    }

}
