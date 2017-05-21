using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class OrganizationDetailViewModel : BaseViewModel
    {
        public OrganizationDetailViewModel(Organization item = null)
        {
            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(async () => await Delete());

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
        async Task Save()
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
                MessagingCenter.Send<OrganizationDetailViewModel>(this, "ItemsChanged");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationDetail] Save error: {ex.Message}");
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
                    var table = await CloudService.GetTableAsync<Organization>();
                    await table.DeleteItemAsync(Item);
                    await CloudService.SyncOfflineCacheAsync<Organization>(overrideServerChanges: true);
                    MessagingCenter.Send<OrganizationDetailViewModel>(this, "ItemsChanged");                    
                }

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationDetail] Save error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
