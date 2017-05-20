using Microsoft.Azure.Mobile.Server.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace NGODirectory.Backend.Helpers
{
    public class ClaimsHelper
    {
        /// <summary>
        /// Get the list of groups from the claims
        /// </summary>
        /// <returns>The list of groups</returns>
        public async Task<List<string>> GetGroups(HttpRequestMessage request, IPrincipal user)
        {
            var creds = await user.GetAppServiceIdentityAsync<AzureActiveDirectoryCredentials>(request);
            return creds.UserClaims
                .Where(claim => claim.Type.Equals("groups"))
                .Select(claim => claim.Value)
                .ToList();
        }
    }
}