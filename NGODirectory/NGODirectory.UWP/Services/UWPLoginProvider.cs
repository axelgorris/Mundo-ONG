using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NGODirectory.Abstractions;
using NGODirectory.UWP.Services;
using System;
using NGODirectory.Helpers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(UWPLoginProvider))]
namespace NGODirectory.UWP.Services
{
    public class UWPLoginProvider : ILoginProvider
    {
        /// <summary>
        /// Login via ADAL
        /// </summary>
        /// <returns>(async) token from the ADAL process</returns>
        public async Task<string> LoginADALAsync()
        {
            Uri returnUri = new Uri(Locations.AadRedirectUri);
            var authContext = new AuthenticationContext(Locations.AadAuthority);

            if (authContext.TokenCache.ReadItems().Count() > 0)
            {
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);
            }

            var authResult = await authContext.AcquireTokenAsync(
                    Locations.AppServiceUrl, /* The resource we want to access  */
                    Locations.AadClientId,   /* The Client ID of the Native App */
                    returnUri,               /* The Return URI we configured    */
                    new PlatformParameters(PromptBehavior.Auto, false));

            return authResult.AccessToken;
        }

        public async Task LoginAsync(MobileServiceClient client)
        {
            // Client Flow
            //var accessToken = await LoginADALAsync();

            //var zumoPayload = new JObject()
            //{
            //    ["access_token"] = accessToken
            //};

            //await client.LoginAsync("aad", zumoPayload);

            // Server-Flow Version
            await client.LoginAsync("aad");
        }
    }
}