using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Title = "NGO Directory";
        }

        Command logoutCmd;
        public Command LogoutCommand => logoutCmd ?? (logoutCmd = new Command(async () => await ExecuteLogoutCommand()));
        async Task ExecuteLogoutCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
                await cloudService.LogoutAsync();
                Application.Current.MainPage = new NavigationPage(new Views.EntryView());
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
