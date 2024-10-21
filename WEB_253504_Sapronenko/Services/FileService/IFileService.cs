namespace WEB_253504_Sapronenko.UI.Services.FileService
{
    public interface IFileService
    {

        Task<string> SaveFileAsync(IFormFile formFile);

        Task DeleteFileAsync(string fileName);
    }

}
