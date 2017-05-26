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
            PickPhotoCommand = new Command(async () => await PickPhotoAsync());

            if (item != null)
            {
                Item = item;
                Title = item.Name;
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
        
        private string _logoUrl;
        public string LogoUrl
        {
            get { return _logoUrl; }
            set { SetProperty(ref _logoUrl, value, "LogoUrl"); }
        }

        private MediaFile _image;
        public MediaFile Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value, "Image"); }
        }

        public Command PickPhotoCommand { get; }
        private async Task PickPhotoAsync()
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