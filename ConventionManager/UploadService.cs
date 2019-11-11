using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ConventionManager
{
    public class UploadService :IUpload 
    {
        private IConfiguration _configuration;

        public UploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CloudBlobContainer GetPicturesContainer()
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("profile-pictures");
            return container;
        }
        public CloudBlobContainer GetPdfsContainer()
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("pdf-files");
            return container;
        }
    }
}