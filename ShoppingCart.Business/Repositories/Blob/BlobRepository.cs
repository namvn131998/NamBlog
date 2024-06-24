using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Data;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Business.Repositories;
using ShoppingCart.Business.Utilities;
using Azure.Storage.Blobs;

namespace ShoppingCart.Business.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;

        public BlobRepository(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient("namvucontainer");
        }
        public async Task<string> UploadFileToBlobStorage(string filePath, string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            var status = await blobClient.UploadAsync(filePath);

            return blobClient.Uri.AbsoluteUri;
        }
        public void DeleteFileBlobStorage(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            blobClient.DeleteIfExists();
        }
    }
}