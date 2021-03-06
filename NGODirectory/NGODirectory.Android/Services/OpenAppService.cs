﻿using Android.App;
using Android.Content;
using NGODirectory.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.Droid.Services
{
    public class OpenAppService : Activity, IOpenAppService
    {
        public Task<bool> Launch(string stringUri)
        {
            Intent intent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(stringUri);

            if (intent != null)
            {
                intent.AddFlags(ActivityFlags.NewTask);
                Forms.Context.StartActivity(intent);
            }
            else
            {
                intent = new Intent(Intent.ActionView);
                intent.AddFlags(ActivityFlags.NewTask);
                intent.SetData(Android.Net.Uri.Parse("market://details?id=" + stringUri)); // This launches the PlayStore and search for the app if it's not installed on your device
                Forms.Context.StartActivity(intent);
            }

            return Task.FromResult(true);
        }
    }
}