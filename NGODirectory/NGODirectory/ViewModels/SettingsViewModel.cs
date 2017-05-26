using NGODirectory.Abstractions;
using NGODirectory.Helpers;
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
            Title = "Settings";
        }

        ICloudService _cloudService => ServiceLocator.Instance.Resolve<ICloudService>();
        public ICloudService CloudService
        {
            get { return _cloudService; }
        }

        public bool IsUserLoggedIn
        {
            get { return CloudService.IsUserLoggedIn(); }
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

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
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

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Logout Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
