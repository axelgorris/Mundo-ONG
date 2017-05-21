using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.Services
{
    public class AzureCloudService : ICloudService
    {
        MobileServiceClient Client;
        List<AppServiceIdentity> identities = null;
        
        public AzureCloudService()
        {
            Client = new MobileServiceClient(Locations.AppServiceUrl, new AuthenticationDelegatingHandler());
            if (Locations.AlternateLoginHost != null)
                Client.AlternateLoginHost = new Uri(Locations.AlternateLoginHost);
        }

        #region Offline Sync Initialization
        async Task InitializeAsync()
        {
            // Short circuit - local database is already initialized
            if (Client.SyncContext.IsInitialized)
                return;

            // Create a reference to the local sqlite store
            var store = new MobileServiceSQLiteStore("offlinecache.db");

            // Define the database schema
            store.DefineTable<Organization>();
            store.DefineTable<Announcement>();

            // Actually create the store and update the schema
            Task.Run(async () => await Client.SyncContext.InitializeAsync(store)).Wait();
        }

        public async Task SyncOfflineCacheAsync<T>(bool overrideServerChanges) where T : TableData
        {
            Debug.WriteLine("SyncOfflineCacheAsync: Initializing...");
            await InitializeAsync();

            if (!(await CrossConnectivity.Current.IsRemoteReachable(Client.MobileAppUri.Host, 443)))
            {
                Debug.WriteLine($"Cannot connect to {Client.MobileAppUri} right now - offline");
                return;
            }

            // Push the Operations Queue to the mobile backend
            Debug.WriteLine("SyncOfflineCacheAsync: Pushing Changes");

            try
            {
                await Client.SyncContext.PushAsync();
            }
            catch (MobileServicePushFailedException ex)
            {
                if (ex.PushResult != null)
                {
                    foreach (var error in ex.PushResult.Errors)
                    {
                        await ResolveConflictAsync<T>(error, overrideServerChanges);
                    }
                }
            }

            // Pull each sync table
            Debug.WriteLine("SyncOfflineCacheAsync: Pulling organization table");
            var organizationTable = await GetTableAsync<Organization>(); await organizationTable.PullAsync();
            Debug.WriteLine("SyncOfflineCacheAsync: Pulling announcements table");
            var announcementTable = await GetTableAsync<Announcement>(); await announcementTable.PullAsync();
        }

        async Task ResolveConflictAsync<T>(MobileServiceTableOperationError error, bool overrideServerChanges) where T : TableData
        {
            var serverItem = error.Result.ToObject<T>();
            var localItem = error.Item.ToObject<T>();

            // Note that you need to implement the public override Equals(T) method in the Model for this to work
            //if (serverItem.Equals(localItem))
            //{
            //    // Items are the same, so ignore the conflict
            //    await error.CancelAndDiscardItemAsync();
            //    return;
            //}

            if (overrideServerChanges)
            {
                // Client wins
                localItem.Version = serverItem.Version;
                await error.UpdateOperationAsync(JObject.FromObject(localItem));
            }
            else
            {
                // Server wins
                await error.CancelAndDiscardItemAsync();
            }
        }
        #endregion

        public async Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData
        {
            await InitializeAsync();

            return new AzureCloudTable<T>(Client);
        }

        public async Task<MobileServiceUser> LoginAsync()
        {
            var loginProvider = DependencyService.Get<ILoginProvider>();

            Client.CurrentUser = loginProvider.RetrieveTokenFromSecureStore();

            if (Client.CurrentUser != null)
            {
                // User has previously been authenticated - try to Refresh the token
                try
                {
                    var refreshedUSer = await Client.RefreshUserAsync();
                    if (refreshedUSer != null)
                    {
                        loginProvider.StoreTokenInSecureStore(refreshedUSer);

                        return refreshedUSer;
                    }
                }
                catch (Exception refreshException)
                {
                    Debug.WriteLine($"Could not refresh token: {refreshException.Message}");
                }
            }

            if (Client.CurrentUser != null && !IsTokenExpired(Client.CurrentUser.MobileServiceAuthenticationToken))
            {
                // User has previously been authenticated, no refresh is required
                return Client.CurrentUser;
            }

            // We need to ask for credentials at this point
            await loginProvider.LoginAsync(Client);
            if (Client.CurrentUser != null)
            {
                // We were able to successfully log in
                loginProvider.StoreTokenInSecureStore(Client.CurrentUser);
            }

            return Client.CurrentUser;
        }
        
        public async Task LogoutAsync()
        {
            if (Client.CurrentUser == null || Client.CurrentUser.MobileServiceAuthenticationToken == null)
                return;

            // Log out of the identity provider (if required)

            // Invalidate the token on the mobile backend
            var authUri = new Uri($"{Client.MobileAppUri}/.auth/logout");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Client.CurrentUser.MobileServiceAuthenticationToken);
                await httpClient.GetAsync(authUri);
            }

            // Remove the token from the cache
            DependencyService.Get<ILoginProvider>().RemoveTokenFromSecureStore();

            // Remove the token from the MobileServiceClient
            await Client.LogoutAsync();
        }

        bool IsTokenExpired(string token)
        {
            // Get just the JWT part of the token (without the signature).
            var jwt = token.Split(new Char[] { '.' })[1];

            // Undo the URL encoding.
            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch (jwt.Length % 4)
            {
                case 0: break;
                case 2: jwt += "=="; break;
                case 3: jwt += "="; break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            // Convert to a JSON String
            var bytes = Convert.FromBase64String(jwt);
            string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            // Parse as JSON object and get the exp field value,
            // which is the expiration date as a JavaScript primative date.
            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // Calculate the expiration by adding the exp value (in seconds) to the
            // base date of 1/1/1970.
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }

        public async Task<AppServiceIdentity> GetIdentityAsync()
        {
            if (Client.CurrentUser == null || Client.CurrentUser?.MobileServiceAuthenticationToken == null)
            {
                throw new InvalidOperationException("Not Authenticated");
            }

            if (identities == null)
            {
                identities = await Client.InvokeApiAsync<List<AppServiceIdentity>>("/.auth/me");
            }

            if (identities.Count > 0)
                return identities[0];
            return null;
        }
    }
}
