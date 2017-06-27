using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using NGODirectory.Services;
using Plugin.Messaging;
using Plugin.Share;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class OrganizationDisplayViewModel : BaseViewModel
    {
        public OrganizationDisplayViewModel(Organization item)
        {
            item.Description = Regex.Replace(item.Description, @"\r\n?|\n", Environment.NewLine);

            Item = item;
            Title = item.Name;

            EditCommand = new Command(async () => await EditAsync());
            OpenBrowserCommand = new Command<string>(async (param) => await OpenBrowserAsync(param));
            MakePhoneCallCommand = new Command<string>(MakePhoneCall);
            SendEmailCommand = new Command<string>(SendEmail);

            IsOrganizationAdmin = CloudService.IsUserLoggedIn() &&
                                    CloudService.GetCurrentUser().UserId.Equals(Item.AdminUser);

            MessagingCenter.Subscribe<SettingsViewModel>(this, "RefreshLogin", (sender) =>
            {
                IsOrganizationAdmin = CloudService.IsUserLoggedIn() &&
                                    CloudService.GetCurrentUser().UserId.Equals(Item.AdminUser);
            });
        }

        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Organization Item { get; set; }

        public Command EditCommand { get; }
        async Task EditAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await NavigationService.Instance.NavigateTo<OrganizationEditViewModel>(Item);
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


        public Command<string> OpenBrowserCommand { get; }
        async Task OpenBrowserAsync(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!value.StartsWith("http"))
                    value = $"http://{value}";

                await CrossShare.Current.OpenBrowser(value);
            }
                
        }

        public Command<string> MakePhoneCallCommand { get; }
        void MakePhoneCall(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var phoneDialer = CrossMessaging.Current.PhoneDialer;
                if (phoneDialer.CanMakePhoneCall)
                    phoneDialer.MakePhoneCall(value);
            }
        }

        public Command<string> SendEmailCommand { get; }
        void SendEmail(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var emailTask = CrossMessaging.Current.EmailMessenger;
                if (emailTask.CanSendEmail)
                {
                    emailTask.SendEmail(value);
                }
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
