using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class OrganizationsListViewModel : BaseViewModel
    {
        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();

        public OrganizationsListViewModel()
        {
            Title = "Organizaciones";

            RefreshCommand = new Command(async () => await Refresh());
            AddNewItemCommand = new Command(async () => await AddNewItem());
            SettingsCommand = new Command(async () => await GoToSettings());

            MessagingCenter.Subscribe<OrganizationEditViewModel>(this, "ItemsChanged", async (sender) =>
            {
                await Refresh();
            });

            RefreshCommand.Execute(null);
        }

        public override void OnAppearing(object navigationContext)
        {
            IsUserLoggedIn = CloudService.IsUserLoggedIn();
        }

        public ObservableRangeCollection<Organization> ItemsCopy;
        ObservableRangeCollection<Organization> items = new ObservableRangeCollection<Organization>();
        public ObservableRangeCollection<Organization> Items
        {
            get { return items; }
            set { SetProperty(ref items, value, "Items"); }
        }

        Organization selectedItem;
        public Organization SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value, "SelectedItem");
                if (selectedItem != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new Views.OrganizationDisplayView(selectedItem));
                    SelectedItem = null;
                }
            }
        }

        private string searchFilter;
        public string SearchFilter
        {
            get { return searchFilter; }
            set
            {
                searchFilter = value;
                SetProperty(ref searchFilter, value, "SearchFilter");
                FilterItems();
            }
        }

        public Command RefreshCommand { get; }
        async Task Refresh()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await CloudService.SyncOfflineCacheAsync<Organization>(overrideServerChanges: true);
                var table = await CloudService.GetTableAsync<Organization>();

                ICollection<Organization> list;

                list = await table.ReadAllItemsOrderedAsync(o => o.Name);

                Items = new ObservableRangeCollection<Organization>(list);                
                ItemsCopy = Items;
                //SearchFilter = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationsList] Error loading items: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command AddNewItemCommand { get; }
        async Task AddNewItem()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.OrganizationEditView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationsList] Error in AddNewItem: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command SettingsCommand { get; }
        async Task GoToSettings()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.SettingsView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[OrganizationsList] Error in GoToSettings: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool isUserLoggedIn;
        public bool IsUserLoggedIn
        {
            get { return isUserLoggedIn; }
            private set { SetProperty(ref isUserLoggedIn, value, "IsUserLoggedIn"); }
        }

        public bool IsUWPDevice
        {
            get
            {
                return Device.RuntimePlatform.Equals(Device.Windows);
            }
        }

        private void FilterItems()
        {
            if (ItemsCopy != null)
            {
                if (String.IsNullOrEmpty(SearchFilter))
                {
                    Items = ItemsCopy;
                }
                else
                {
                    Items = new ObservableRangeCollection<Organization>(ItemsCopy.Where(x => x.Name.ToLower().Contains(SearchFilter.ToLower())));
                }
            }
        }
    }
}