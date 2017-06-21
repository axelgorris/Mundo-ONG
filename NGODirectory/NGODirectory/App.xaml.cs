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
        public static CustomNavigationPage NavigationPage { get; set; }

        public static bool MenuIsPresented
        {
            get
            {
                return ((MasterDetailPage)(App.Current.MainPage)).IsPresented;
            }
            set
            {
                ((MasterDetailPage)(App.Current.MainPage)).IsPresented = value;
            }
        }

        public App()
        {
            InitializeComponent();

            ServiceLocator.Instance.Add<ICloudService, AzureCloudService>();

            var CloudService = ServiceLocator.Instance.Resolve<ICloudService>();
            CloudService.StoredLoginAsync();
            
            NavigationPage = new CustomNavigationPage(new BarPage());

            var masterPage = new MasterDetailPage();
            masterPage.Master = new MasterPage() { Title = "Ajustes" };
            masterPage.Detail = NavigationPage;
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
