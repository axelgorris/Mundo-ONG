using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using NGODirectory.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthService))]
namespace NGODirectory.Services
{
    public class AuthService : IAuthService
    {
        public Task LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            return client.LoginAsync(provider);
        }
    }
}