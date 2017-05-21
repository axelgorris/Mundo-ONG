using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public SettingsPageViewModel()
        {
            Title = "NGO Directory";
            
            LoginCommand = new Command(async () => await Login().ConfigureAwait(false));
            LogoutCommand = new Command(async () => await Logout());
        }

        public Command LoginCommand;
        async Task Login()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                await cloudService.LoginAsync();

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

        public Command LogoutCommand { get; }
        async Task Logout()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                await cloudService.LogoutAsync();
                Application.Current.MainPage = new NavigationPage(new Views.SettingsView());
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
