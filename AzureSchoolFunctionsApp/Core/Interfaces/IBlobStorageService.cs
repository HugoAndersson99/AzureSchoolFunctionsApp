

namespace AzureSchoolFunctionsApp.Core.Interfaces
{
    public interface IBlobStorageService
    {
        Task UploadFileAsync(string containerName, string fileName, string content);
    }
}
