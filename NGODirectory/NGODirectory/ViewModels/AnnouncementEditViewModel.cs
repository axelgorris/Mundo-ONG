using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using NGODirectory.Services;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class AnnouncementEditViewModel : BaseViewModel
    {
        public AnnouncementEditViewModel(Announcement item = null)
        {
            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());
            PickImageCommand = new Command(async () => await PickImageAsync());

            if (item != null)
            {
                Item = item;
                Title = "Editar noticia";
                ImageUrl = item.ImageUrl;
                IsEditMode = true;
            }
            else
            {
                Item = new Announcement();
                Title = "Nueva noticia";
                IsEditMode = false;
            }
        }

        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Announcement Item { get; set; }

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
                    ImageUrl = await CloudService.UploadStreamAsync(CloudService.GetCurrentUser().UserId, Image.GetStream());
                    Item.ImageUrl = ImageUrl;
                }

                var table = await CloudService.GetTableAsync<Announcement>();

                if (Item.Id == null)
                {
                    await table.CreateItemAsync(Item);
                }
                else
                {
                    await table.UpdateItemAsync(Item);
                }
                
                await CloudService.SyncOfflineCacheAsync<Announcement>(overrideServerChanges: true);
                MessagingCenter.Send(this, "ItemsChanged");
                await Application.Current.MainPage.Navigation.PopToRootAsync();
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
                    var table = await CloudService.GetTableAsync<Announcement>();
                    await table.DeleteItemAsync(Item);
                    await CloudService.SyncOfflineCacheAsync<Announcement>(overrideServerChanges: true);
                    MessagingCenter.Send(this, "ItemsChanged");
                }

                await Application.Current.MainPage.Navigation.PopToRootAsync();
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

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { SetProperty(ref imageUrl, value, "ImageUrl"); }
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
                ImageUrl = result.Path;
            }
        }

        public bool IsEditMode { get; private set; }
    }
}
