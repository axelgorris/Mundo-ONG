using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using NGODirectory.Services;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class OrganizationEditViewModel : BaseViewModel
    {
        public OrganizationEditViewModel(Organization item = null)
        {
            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());
            PickImageCommand = new Command(async () => await PickImageAsync());

            if (item != null)
            {
                Item = item;
                Title = item.Name;
                LogoUrl = item.LogoUrl;
            }
            else
            {
                Item = new Organization();
                Title = "New organization";
            }
        }

        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Organization Item { get; set; }

        public Command SaveCommand { get; }
        async Task SaveAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (Image != null)
                {
                    LogoUrl = await CloudService.UploadStreamAsync(Item.Id, Image.GetStream());
                    Item.LogoUrl = LogoUrl;
                }

                var table = await CloudService.GetTableAsync<Organization>();

                if (Item.Id == null)
                {
                    await table.CreateItemAsync(Item);
                }
                else
                {
                    await table.UpdateItemAsync(Item);
                }

                await CloudService.SyncOfflineCacheAsync<Organization>(overrideServerChanges: true);
                MessagingCenter.Send(this, "ItemsChanged");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationEdit] Save error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command DeleteCommand { get; }
        async Task DeleteAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (Item.Id != null)
                {
                    var table = await CloudService.GetTableAsync<Organization>();
                    await table.DeleteItemAsync(Item);
                    await CloudService.SyncOfflineCacheAsync<Organization>(overrideServerChanges: true);
                    MessagingCenter.Send(this, "ItemsChanged");
                }

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationEdit] Save error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        private string logoUrl;
        public string LogoUrl
        {
            get { return logoUrl; }
            set { SetProperty(ref logoUrl, value, "LogoUrl"); }
        }

        private MediaFile image;
        public MediaFile Image
        {
            get { return image; }
            set { SetProperty(ref image, value, "Image"); }
        }

        public Command PickImageCommand { get; }
        private async Task PickImageAsync()
        {
            var result = await PhotoService.Instance.PickPhotoAsync();

            if (result != null)
            {
                Image = result;
                LogoUrl = result.Path;
            }
        }
    }
}