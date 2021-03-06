﻿using NGODirectory.Abstractions;
using NGODirectory.UWP.Services;
using System;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppService))]
namespace NGODirectory.UWP.Services
{
    public class OpenAppService : IOpenAppService
    {
        public async Task<bool> Launch(string stringUri)
        {
            Uri uri = new Uri(stringUri);

            return await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
