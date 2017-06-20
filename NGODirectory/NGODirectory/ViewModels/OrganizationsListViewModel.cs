using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using NGODirectory.Models;
using System;
using System.Collections.Generic;
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

        public OrganizationsListViewModel()
        {
            Title = "Organizaciones";

            RefreshCommand = new Command(async () => await Refresh());
            AddNewItemCommand = new Command(async () => await AddNewItem());
            IsUserLoggedIn = CloudService.IsUserLoggedIn();

            MessagingCenter.Subscribe<OrganizationEditViewModel>(this, "ItemsChanged", async (sender) =>
            {
                await Refresh();
            });

            MessagingCenter.Subscribe<MasterModel>(this, "RefreshLogin", (sender) =>
            {
                IsUserLoggedIn = CloudService.IsUserLoggedIn();
            });

            RefreshCommand.Execute(null);
        }
       
        public ObservableCollection<Organization> ItemsCopy { get; set; }
        ObservableCollection<Grouping<string, Organization>> itemsGrouped = new ObservableCollection<Grouping<string, Organization>>();
        public ObservableCollection<Grouping<string, Organization>> ItemsGrouped
        {
            get { return itemsGrouped; }
            set { SetProperty(ref itemsGrouped, value, "ItemsGrouped"); }
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
                    App.NavigationPage.PushAsync(new Pages.OrganizationDisplayPage(selectedItem));
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

                var sorted = from item in list
                             orderby item.Name
                             group item by item.Name.Substring(0, 1).ToUpper() into itemGroup
                             select new Grouping<string, Organization>(itemGroup.Key, itemGroup);

                ItemsGrouped = new ObservableCollection<Grouping<string, Organization>>(sorted);
                ItemsCopy = new ObservableCollection<Organization>(list);
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
                await App.NavigationPage.PushAsync(new Pages.OrganizationEditPage());
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
                ObservableCollection<Organization> source;

                if (String.IsNullOrEmpty(SearchFilter))
                {
                    source = ItemsCopy;
                }
                else
                {
                    source = new ObservableRangeCollection<Organization>(ItemsCopy.Where(x => x.Name.ToUpper()
                                                                                               .Contains(SearchFilter.ToUpper())));
                }

                var sorted = from item in source
                             orderby item.Name
                             group item by item.Name.Substring(0, 1).ToUpper() into itemGroup
                             select new Grouping<string, Organization>(itemGroup.Key, itemGroup);

                ItemsGrouped = new ObservableCollection<Grouping<string, Organization>>(sorted);
            }
        }
    }
}