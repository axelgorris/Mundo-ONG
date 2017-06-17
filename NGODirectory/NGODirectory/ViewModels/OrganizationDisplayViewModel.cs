using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class OrganizationDisplayViewModel : BaseViewModel
    {
        public OrganizationDisplayViewModel(Organization item)
        {
            EditCommand = new Command(async () => await Edit());

            Item = item;
            Title = item.Name;

            IsOrganizationAdmin = CloudService.IsUserLoggedIn() && CloudService.GetCurrentUser().UserId.Equals(Item.AdminUser);
        }
        
        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Organization Item { get; set; }

        public Command EditCommand { get; }
        async Task Edit()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.OrganizationEditView(Item));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationDisplay] Edit error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool isOrganizationAdmin;
        public bool IsOrganizationAdmin
        {
            get { return isOrganizationAdmin; }
            private set { SetProperty(ref isOrganizationAdmin, value, "IsOrganizationAdmin"); }
        }
    }
}
