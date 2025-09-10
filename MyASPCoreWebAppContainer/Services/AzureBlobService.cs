using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MyASPCoreWebAppContainer.Services
{
    public class AzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AzureBlobService(string connectionString, string containerName)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = containerName;
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            //await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            var blobClient = containerClient.GetBlobClient(file.FileName);

            await blobClient.UploadAsync(file.OpenReadStream(), true);
        }

        // New method to list all files
        public async Task<List<string>> ListFilesAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var files = new List<string>();

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                files.Add(blobItem.Name);
            }

            return files;
        }

        public async Task<BlobDownloadResult> DownloadFileAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.DownloadContentAsync();
        }
    }
}
