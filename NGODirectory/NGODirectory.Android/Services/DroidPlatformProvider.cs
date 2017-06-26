using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using NGODirectory.Abstractions;
using NGODirectory.Droid.Services;
using System.Threading.Tasks;
using Xamarin.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(DroidPlatformProvider))]
namespace NGODirectory.Droid.Services
{
    public class DroidPlatformProvider : IPlatformProvider
    {
        public Context RootView { get; private set; }

        public AccountStore AccountStore { get; private set; }

        public void Init(Context context)
        {
            RootView = context;
            AccountStore = AccountStore.Create(context);            
        }

        #region ILoginProvider Interface
        public MobileServiceUser RetrieveTokenFromSecureStore()
        {
            string token;
            var accounts = AccountStore.FindAccountsForService("NGODirectory");

            if (accounts != null)
            {
                foreach (var account in accounts)
                {
                    if (account.Properties.TryGetValue("token", out token))
                    {
                        return new MobileServiceUser(account.Username)
                        {
                            MobileServiceAuthenticationToken = token
                        };
                    }
                }
            }

            return null;
        }

        public void StoreTokenInSecureStore(MobileServiceUser user)
        {
            var account = new Account(user.UserId);
            account.Properties.Add("token", user.MobileServiceAuthenticationToken);
            AccountStore.Save(account, "NGODirectory");
        }

        public void RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService("NGODirectory");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    AccountStore.Delete(acct, "NGODirectory");
                }
            }
        }

        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        {
            // Server Flow
            return await client.LoginAsync(RootView, "aad");
        }        
        #endregion
    }
}