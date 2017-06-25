using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Pages;
using NGODirectory.Services;
using Plugin.Share;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            Title = "Ajustes";

            LoginCommand = new Command(async () => await LoginAsync());
            LogoutCommand = new Command(async () => await LogoutAsync());
            OpenBrowserCommand = new Command<string>(async (param) => await OpenBrowserAsync(param));

            IsUserLoggedIn = CloudService.IsUserLoggedIn();
        }

        private ICloudService cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
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

        public Command LoginCommand { get; }
        async Task LoginAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await CloudService.LoginAsync();
                SetProperty(ref isUserLoggedIn, CloudService.IsUserLoggedIn(), "IsUserLoggedIn");
                MessagingCenter.Send(this, "RefreshLogin");
                NavigationService.Instance.MenuIsPresented = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error al iniciar sesión", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command LogoutCommand { get; }
        async Task LogoutAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await CloudService.LogoutAsync();
                SetProperty(ref isUserLoggedIn, CloudService.IsUserLoggedIn(), "IsUserLoggedIn");
                MessagingCenter.Send(this, "RefreshLogin");
                NavigationService.Instance.MenuIsPresented = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error al cerar sesión", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command OpenBrowserCommand { get; }
        async Task OpenBrowserAsync(string value)
        {
            if (!string.IsNullOrEmpty(value))
                await CrossShare.Current.OpenBrowser(value);
        }

        public bool EnableNotifications
        {
            get { return Settings.EnableNotifications; }
            set { Settings.EnableNotifications = value; }
        }

        public bool DownloadOnlyOnWifi
        {
            get { return Settings.DownloadOnlyOnWifi; }
            set { Settings.DownloadOnlyOnWifi = value; }
        }
    }
}
