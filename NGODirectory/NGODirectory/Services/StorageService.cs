using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Plugin.Media.Abstractions;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Blob;
using NGODirectory.Models;
using System.Collections.Generic;
using System.Net.Http;
using NGODirectory.Abstractions;
using System.IO;

namespace NGODirectory.Services
{
    public class StorageService
    {
        private static StorageService _instance;
        
        public static StorageService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StorageService();
                }

                return _instance;
            }
        }


        private async Task<StorageTokenViewModel> GetSasTokenAsync(MobileServiceClient client, string directoryName)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("directoryName", directoryName);
            var storageToken = await client.InvokeApiAsync<StorageTokenViewModel>("GetStorageToken", HttpMethod.Get, parameters);
            return storageToken;
        }

        public async Task<string> UploadStreamAsync(MobileServiceClient client, string directoryName, Stream image)
        {
            // Get the SAS token from the backend
            var storageToken = await GetSasTokenAsync(client, directoryName);

            // Use the SAS token to upload the file
            var storageUri = new Uri($"{storageToken.Uri}{storageToken.SasToken}");
            var blobStorage = new CloudBlockBlob(storageUri);
            blobStorage.Properties.ContentType = "image/png";
            await blobStorage.UploadFromStreamAsync(image);
            
            return storageToken.Uri.ToString();            
        }
    }
}
