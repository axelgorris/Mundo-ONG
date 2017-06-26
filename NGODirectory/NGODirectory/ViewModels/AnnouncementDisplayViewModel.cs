using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using NGODirectory.Services;
using Plugin.Share;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class AnnouncementDisplayViewModel : BaseViewModel
    {
        public AnnouncementDisplayViewModel(Announcement item)
        {
            Title = string.Empty;            
            //item.Description = Regex.Replace(item.Description, @"\r\n?|\n", Environment.NewLine);

            Item = item;            

            EditCommand = new Command(async () => await EditAsync());
            OpenBrowserCommand = new Command<string>(async (param) => await OpenBrowserAsync(param));

            IsAuthor = CloudService.IsUserLoggedIn() &&
                        CloudService.GetCurrentUser().UserId.Equals(Item.Author);

            MessagingCenter.Subscribe<SettingsViewModel>(this, "RefreshLogin", (sender) =>
            {
                IsAuthor = CloudService.IsUserLoggedIn() && 
                            CloudService.GetCurrentUser().UserId.Equals(Item.Author);
            });
        }
        
        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Announcement Item { get; set; }

        public Command EditCommand { get; }
        async Task EditAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await NavigationService.Instance.NavigateTo<AnnouncementEditViewModel>(Item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AnnouncementEdit] Save error: {ex.Message}");
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
            {
                if (!value.StartsWith("http"))
                    value = $"http://{value}";

                await CrossShare.Current.OpenBrowser(value);
            }
        }

        private bool isAuthor;
        public bool IsAuthor
        {
            get { return isAuthor; }
            private set { SetProperty(ref isAuthor, value, "IsAuthor"); }
        }
    }
}
