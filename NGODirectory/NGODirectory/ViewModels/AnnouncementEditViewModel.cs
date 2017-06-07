using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
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
            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(async () => await Delete());

            if (item != null)
            {
                Item = item;
                Title = item.Title;
            }
            else
            {
                Item = new Announcement();
                Title = "New announcement";
            }
        }

        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public Announcement Item { get; set; }

        public Command SaveCommand { get; }
        async Task Save()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
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
                await Application.Current.MainPage.Navigation.PopAsync();
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
        async Task Delete()
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
    }
}
