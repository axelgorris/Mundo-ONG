﻿using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace NGODirectory.Abstractions
{
    public interface IPlatformProvider
    {
        MobileServiceUser RetrieveTokenFromSecureStore();

        void StoreTokenInSecureStore(MobileServiceUser user);

        void RemoveTokenFromSecureStore();

        Task<MobileServiceUser> LoginAsync(MobileServiceClient client);
    }
}
