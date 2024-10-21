using System.Net.Http.Headers;
using System.Text;

namespace WEB_253504_Sapronenko.UI.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        public ApiFileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task DeleteFileAsync(string fileUri)
        {
            var fileName = Path.GetFileName(fileUri);
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}imageupload/{fileName}").ToString();
            var response = await _httpClient.DeleteAsync(urlString);
        }
        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}imageupload").ToString();

            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            var content = new MultipartFormDataContent();

            var streamContent = new StreamContent(formFile.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);

            content.Add(streamContent, "file", newName);

            var response = await _httpClient.PostAsync(urlString, content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return String.Empty;
        }

    }
}
