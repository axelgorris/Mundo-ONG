using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NGODirectory.Abstractions;
using NGODirectory.UWP.Services;
using System;
using NGODirectory.Helpers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using System.Linq;
using Windows.Security.Credentials;
using System.Diagnostics;

[assembly: Xamarin.Forms.Dependency(typeof(UWPPlatformProvider))]
namespace NGODirectory.UWP.Services
{
    public class UWPPlatformProvider : IPlatformProvider
    {
        public PasswordVault PasswordVault { get; private set; }

        public UWPPlatformProvider()
        {
            PasswordVault = new PasswordVault();
        }

        #region ILoginProvider Interface
        public MobileServiceUser RetrieveTokenFromSecureStore()
        {
            try
            {
                // Check if the token is available within the password vault
                var accounts = PasswordVault.FindAllByResource("NGODirectory");
                if (accounts != null)
                {
                    foreach (var account in accounts)
                    {
                        var token = PasswordVault.Retrieve("NGODirectory", account.UserName).Password;
                        if (token != null && token.Length > 0)
                        {
                            return new MobileServiceUser(account.UserName)
                            {
                                MobileServiceAuthenticationToken = token
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving existing token: {ex.Message}");
            }
            return null;
        }

        public void StoreTokenInSecureStore(MobileServiceUser user)
        {
            PasswordVault.Add(new PasswordCredential("NGODirectory", user.UserId, user.MobileServiceAuthenticationToken));
        }

        public void RemoveTokenFromSecureStore()
        {
            var accounts = PasswordVault.FindAllByResource("NGODirectory");

            if (accounts != null)
            {
                foreach (var account in accounts)
                {
                    PasswordVault.Remove(account);
                }
            }
        }

        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        {
            // Server-Flow Version
            return await client.LoginAsync("aad");
        }        
        #endregion
    }
}