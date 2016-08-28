namespace Blongo
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class AzureBlobStorage
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        public AzureBlobStorage(string connectionString)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public async Task DeleteBlob(string containerName, string blobName)
        {
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            var blob = blobContainer.GetBlockBlobReference(blobName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<CloudBlockBlob> GetBlobAsync(string containerName, string blobName)
        {
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);

            if (!await blobContainer.ExistsAsync())
            {
                return null;
            }

            return blobContainer.GetBlockBlobReference(blobName);
        }

        public async Task<IEnumerable<IListBlobItem>> GetBlobsAsync(string containerName, string blobName = null)
        {
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);

            if (!await blobContainer.ExistsAsync())
            {
                return new IListBlobItem[0];
            }

            return blobContainer.ListBlobs(blobName);
        }

        public async Task Purge(string containerName)
        {
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            await blobContainer.DeleteIfExistsAsync();
        }

        public async Task<CloudBlockBlob> SaveBlobAsync(string containerName, Stream blobStream, string blobName)
        {
            var blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            var blob = blobContainer.GetBlockBlobReference(blobName);

            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(blobName, out contentType);

            if (contentType != null)
            {
                blob.Properties.ContentType = contentType;
            }

            await blob.UploadFromStreamAsync(blobStream);

            return blob;
        }
    }
}