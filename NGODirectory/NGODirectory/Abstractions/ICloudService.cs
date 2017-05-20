using Microsoft.WindowsAzure.MobileServices;
using NGODirectory.Models;
using System.Threading.Tasks;

namespace NGODirectory.Abstractions
{
    public interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;

        Task<MobileServiceUser> LoginAsync();

        Task LogoutAsync();

        Task<AppServiceIdentity> GetIdentityAsync();
    }
}
