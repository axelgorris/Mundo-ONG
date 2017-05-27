using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.ViewModels
{
    public class OrganizationsListViewModel : BaseViewModel
    {
        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();
        bool hasMoreItems = true;

        public OrganizationsListViewModel()
        {
            Title = "Organizations";

            RefreshCommand = new Command(async () => await Refresh());
            AddNewItemCommand = new Command(async () => await AddNewItem());
            LoadMoreCommand = new Command<Organization>(async (Organization item) => await LoadMore(item));
            SettingsCommand = new Command(async () => await GoToSettings());
            
            // Subscribe to events from the Task Detail Page
            MessagingCenter.Subscribe<OrganizationEditViewModel>(this, "ItemsChanged", async (sender) =>
            {
                await Refresh();
            });

            // Execute the refresh command
            RefreshCommand.Execute(null);
        }

        public override void OnAppearing(object navigationContext)
        {
            IsUserLoggedIn = CloudService.IsUserLoggedIn();
        }

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
                var list = await table.ReadItemsOrderedAsync(0, 10, o => o.Name);
                Items = new ObservableRangeCollection<Organization>(list);

                hasMoreItems = true;
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

        public Command LoadMoreCommand { get; }
        async Task LoadMore(Organization item)
        {
            if (item == null)
                return;

            if (IsBusy)
            {
                Debug.WriteLine($"[OrganizationsList] LoadMore: bailing because IsBusy = true");
                return;
            }

            // If we are not displaying the last one in the list, then return.
            if (!Items.Last().Id.Equals(item.Id))
            {
                Debug.WriteLine($"[OrganizationsList] LoadMore: bailing because this id is not the last id in the list");
                return;
            }

            // If we don't have more items, return
            if (!hasMoreItems)
            {
                Debug.WriteLine($"[OrganizationsList] LoadMore: bailing because we don't have any more items");
                return;
            }

            IsBusy = true;
            try
            {
                var table = await CloudService.GetTableAsync<Organization>();
                var list = await table.ReadItemsOrderedAsync(Items.Count, 10, o => o.Name);
                if (list.Count > 0)
                {
                    Debug.WriteLine($"[OrganizationsList] LoadMore: got {list.Count} more items");
                    Items.AddRange(list);
                }
                else
                {
                    Debug.WriteLine($"[OrganizationsList] LoadMore: no more items: setting hasMoreItems= false");
                    hasMoreItems = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("[OrganizationsList] LoadMore Failed", ex.Message, "OK");
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
    }
}