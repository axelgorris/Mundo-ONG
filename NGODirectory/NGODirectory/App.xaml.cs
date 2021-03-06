﻿using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Services;
using NGODirectory.Pages;
using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace NGODirectory
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ServiceLocator.Instance.Add<ICloudService, AzureCloudService>();

            var CloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            CloudService.StoredLoginAsync();

            var masterPage = new MasterDetailPage()
            {
                Master = new SettingsPage() { Title = "Ajustes" },
                Detail = new CustomNavigationPage(new BarPage())
            };
            masterPage.MasterBehavior = MasterBehavior.Popover;

            MainPage = masterPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            MobileCenter.Start("android=7dc4a75f-7ea8-4511-9002-a20aaa85ca46;" +
                   "uwp=cb656d1d-6e39-4759-87d2-8a530c235a39;" +
                   "ios=f350b77e-3300-4783-8252-fd07ca72f473;",
                   typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
