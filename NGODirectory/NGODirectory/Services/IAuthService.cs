using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace NGODirectory.Services
{
    public interface IAuthService
    {
        Task LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);
    }
}