using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
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
            Item = item;
            
            EditCommand = new Command(async () => await Edit());
        }

        public override void OnAppearing(object navigationContext)
        {
            IsAuthor = CloudService.IsUserLoggedIn() && CloudService.GetCurrentUser().UserId.Equals(Item.Author);
        }

        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Announcement Item { get; set; }

        public Command EditCommand { get; }
        async Task Edit()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.AnnouncementEditView(Item));
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

        private bool isAuthor;
        public bool IsAuthor
        {
            get { return isAuthor; }
            private set { SetProperty(ref isAuthor, value, "IsAuthor"); }
        }
    }
}
