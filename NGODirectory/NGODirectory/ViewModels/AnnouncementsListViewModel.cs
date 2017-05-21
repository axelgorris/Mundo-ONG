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
    public class AnnouncementsListViewModel : BaseViewModel
    {
        public ICloudService CloudService => ServiceLocator.Instance.Resolve<ICloudService>();
        bool hasMoreItems = true;

        public AnnouncementsListViewModel()
        {
            Title = "Announcements";

            RefreshCommand = new Command(async () => await Refresh());
            AddNewItemCommand = new Command(async () => await AddNewItem());
            LoadMoreCommand = new Command<Announcement>(async (Announcement item) => await LoadMore(item));
            SettingsCommand = new Command(async () => await GoToSettings());
            OrganizationsCommand = new Command(async () => await GoToOrganizations());

            // Subscribe to events from the Task Detail Page
            MessagingCenter.Subscribe<AnnouncementEditViewModel>(this, "ItemsChanged", async (sender) =>
            {
                await Refresh();
            });

            // Execute the refresh command
            RefreshCommand.Execute(null);
        }

        ObservableRangeCollection<Announcement> items = new ObservableRangeCollection<Announcement>();
        public ObservableRangeCollection<Announcement> Items
        {
            get { return items; }
            set { SetProperty(ref items, value, "Items"); }
        }

        Announcement selectedItem;
        public Announcement SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value, "SelectedItem");
                if (selectedItem != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new Views.AnnouncementEditView(selectedItem));
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
                await CloudService.SyncOfflineCacheAsync<Announcement>(overrideServerChanges: true);
                var table = await CloudService.GetTableAsync<Announcement>();
                var list = await table.ReadItemsOrderedAsync(0, 10, o => o.CreatedAt, descending: true);
                Items.ReplaceRange(list);
                hasMoreItems = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AnnouncementsList] Error loading items: {ex.Message}");
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
                await Application.Current.MainPage.Navigation.PushAsync(new Views.AnnouncementEditView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskList] Error in AddNewItem: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command LoadMoreCommand { get; }
        async Task LoadMore(Announcement item)
        {
            if (IsBusy)
            {
                Debug.WriteLine($"[AnnouncementsList] LoadMore: bailing because IsBusy = true");
                return;
            }

            // If we are not displaying the last one in the list, then return.
            if (!Items.Last().Id.Equals(item.Id))
            {
                Debug.WriteLine($"[AnnouncementsList] LoadMore: bailing because this id is not the last id in the list");
                return;
            }

            // If we don't have more items, return
            if (!hasMoreItems)
            {
                Debug.WriteLine($"[AnnouncementsList] LoadMore: bailing because we don't have any more items");
                return;
            }

            IsBusy = true;
            try
            {
                var table = await CloudService.GetTableAsync<Announcement>();
                var list = await table.ReadItemsOrderedAsync(Items.Count, 10, o => o.CreatedAt, descending: true);
                if (list.Count > 0)
                {
                    Debug.WriteLine($"[AnnouncementsList] LoadMore: got {list.Count} more items");
                    Items.AddRange(list);
                }
                else
                {
                    Debug.WriteLine($"[AnnouncementsList] LoadMore: no more items: setting hasMoreItems= false");
                    hasMoreItems = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("[AnnouncementsList] LoadMore Failed", ex.Message, "OK");
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
                Debug.WriteLine($"[TaskList] Error in GoToSettings: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command OrganizationsCommand { get; }
        async Task GoToOrganizations()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.OrganizationsListView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TaskList] Error in GoToOrganizations: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
