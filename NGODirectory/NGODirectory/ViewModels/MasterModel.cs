using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Pages;
using Plugin.Share;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class MasterModel : BaseViewModel
    {
        public MasterModel()
        {
            Title = "Ajustes";

            IsUserLoggedIn = CloudService.IsUserLoggedIn();
        }

        ICloudService cloudService => ServiceLocator.Instance.Resolve<ICloudService>();
        public ICloudService CloudService
        {
            get { return cloudService; }
        }

        private bool isUserLoggedIn;
        public bool IsUserLoggedIn
        {
            get { return isUserLoggedIn; }
            set { isUserLoggedIn = value; }
        }

        public ICommand LoginCommand => new Command(async () => await Login());
        async Task Login()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await CloudService.LoginAsync();
                SetProperty(ref isUserLoggedIn, CloudService.IsUserLoggedIn(), "IsUserLoggedIn");
                MessagingCenter.Send(this, "RefreshLogin");
                App.MenuIsPresented = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Login failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand LogoutCommand => new Command(async () => await Logout());
        async Task Logout()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await CloudService.LogoutAsync();
                SetProperty(ref isUserLoggedIn, CloudService.IsUserLoggedIn(), "IsUserLoggedIn");
                MessagingCenter.Send(this, "RefreshLogin");
                App.MenuIsPresented = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Logout failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand OpenAppCommand => new Command(async () => await OpenApp());
        async Task OpenApp()
        {
            //var openAppService = DependencyService.Get<IOpenAppService>();
            //await openAppService.Launch("twitter://user?user_id=24221652");

            //Device.OpenUri(new Uri("https://twitter.com/AxelGorris"));

            await CrossShare.Current.OpenBrowser("https://twitter.com/AxelGorris");
        }
    }
}
