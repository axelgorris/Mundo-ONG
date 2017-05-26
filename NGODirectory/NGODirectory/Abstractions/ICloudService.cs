using Microsoft.WindowsAzure.MobileServices;
using NGODirectory.Models;
using System.Threading.Tasks;

namespace NGODirectory.Abstractions
{
    public interface ICloudService
    {
        Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData;
        Task<MobileServiceUser> LoginAsync();
        Task LogoutAsync();
        Task<AppServiceIdentity> GetIdentityAsync();
        Task SyncOfflineCacheAsync<T>(bool overrideServerChanges) where T : TableData;
        bool IsUserLoggedIn();
    }
}
