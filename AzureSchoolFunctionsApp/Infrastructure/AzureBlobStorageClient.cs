using Azure.Storage.Blobs;
using AzureSchoolFunctionsApp.Core.Interfaces;
using System.Text;

namespace AzureSchoolFunctionsApp.Infrastructure
{
    public class AzureBlobStorageClient : IBlobStorageService
    {
        private readonly string _connectionString;

        public AzureBlobStorageClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task UploadFileAsync(string containerName, string fileName, string content)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                await blobClient.UploadAsync(stream, true);
            }
        }
    }
}
