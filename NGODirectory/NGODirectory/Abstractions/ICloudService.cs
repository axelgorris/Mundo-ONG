using Microsoft.WindowsAzure.MobileServices;
using NGODirectory.Models;
using System.IO;
using System.Threading.Tasks;

namespace NGODirectory.Abstractions
{
    public interface ICloudService
    {
        Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData;
        Task<MobileServiceUser> StoredLoginAsync();
        Task<MobileServiceUser> LoginAsync();
        Task LogoutAsync();
        Task<AppServiceIdentity> GetIdentityAsync();
        Task SyncOfflineCacheAsync<T>(bool overrideServerChanges) where T : TableData;        
        Task<string> UploadStreamAsync(string directoryName, Stream image);
        bool IsUserLoggedIn();
        MobileServiceUser GetCurrentUser();
        MobileServiceClient GetMobileServiceClient();
    }
}
