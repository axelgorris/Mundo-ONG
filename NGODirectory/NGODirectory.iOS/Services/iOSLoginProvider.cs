﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.iOS.Services;
using UIKit;
using Xamarin.Auth;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(iOSLoginProvider))]
namespace NGODirectory.iOS.Services
{
    public class iOSLoginProvider : ILoginProvider
    {
        public UIViewController RootView => UIApplication.SharedApplication.KeyWindow.RootViewController;

        //public AccountStore AccountStore { get; private set; }

        //public iOSLoginProvider()
        //{
        //    AccountStore = AccountStore.Create();
        //}

        #region ILoginProvider Interface
        public MobileServiceUser RetrieveTokenFromSecureStore()
        {
            //string token;
            //var accounts = AccountStore.FindAccountsForService("NGODirectory");

            //if (accounts != null)
            //{
            //    foreach (var account in accounts)
            //    {
            //        if (account.Properties.TryGetValue("token", out token))
            //        {
            //            return new MobileServiceUser(account.Username)
            //            {
            //                MobileServiceAuthenticationToken = token
            //            };
            //        }
            //    }
            //}

            return null;
        }

        public void StoreTokenInSecureStore(MobileServiceUser user)
        {
            //var account = new Account(user.UserId);
            //account.Properties.Add("token", user.MobileServiceAuthenticationToken);
            //AccountStore.Save(account, "NGODirectory");
        }

        public void RemoveTokenFromSecureStore()
        {
            //var accounts = AccountStore.FindAccountsForService("tasklist");
            //if (accounts != null)
            //{
            //    foreach (var acct in accounts)
            //    {
            //        AccountStore.Delete(acct, "tasklist");
            //    }
            //}
        }

        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        {
            // Server Flow
            return await client.LoginAsync(RootView, "aad");
        }
        #endregion
    }
}