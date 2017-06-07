using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Services;
using NGODirectory.Views;
using Xamarin.Forms;

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

            MainPage = new CustomNavigationPage(new MainView());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
