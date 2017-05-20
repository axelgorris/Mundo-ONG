﻿using NGODirectory.Abstractions;
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
        public ICloudTable<Organization> CloudTable; 
        bool hasMoreItems = true;

        public OrganizationsListViewModel()
        {
            Title = "Organizations list";

            CloudTable = CloudService.GetTable<Organization>();

            RefreshCommand = new Command(async () => await Refresh());
            AddNewItemCommand = new Command(async () => await AddNewItem());
            LoadMoreCommand = new Command<Organization>(async (Organization item) => await LoadMore(item));


            // Subscribe to events from the Task Detail Page
            MessagingCenter.Subscribe<OrganizationDetailViewModel>(this, "ItemsChanged", async (sender) =>
            {
                await Refresh();
            });

            // Execute the refresh command
            RefreshCommand.Execute(null);
        }

        public Command RefreshCommand { get; }
        public Command AddNewItemCommand { get; }
        public Command LoadMoreCommand { get; }
        
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
                    Application.Current.MainPage.Navigation.PushAsync(new Views.OrganizationDetailView(selectedItem));
                    SelectedItem = null;
                }
            }
        }
        
        async Task Refresh()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var list = await CloudTable.ReadItemsAsync(0, 10);
                Items.ReplaceRange(list);
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

        async Task AddNewItem()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.OrganizationDetailView());
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
        
        async Task LoadMore(Organization item)
        {
            if (IsBusy)
            {
                Debug.WriteLine($"LoadMore: bailing because IsBusy = true");
                return;
            }

            // If we are not displaying the last one in the list, then return.
            if (!Items.Last().Id.Equals(item.Id))
            {
                Debug.WriteLine($"LoadMore: bailing because this id is not the last id in the list");
                return;
            }

            // If we don't have more items, return
            if (!hasMoreItems)
            {
                Debug.WriteLine($"LoadMore: bailing because we don't have any more items");
                return;
            }

            IsBusy = true;
            try
            {
                var list = await CloudTable.ReadItemsAsync(Items.Count, 20);
                if (list.Count > 0)
                {
                    Debug.WriteLine($"LoadMore: got {list.Count} more items");
                    Items.AddRange(list);
                }
                else
                {
                    Debug.WriteLine($"LoadMore: no more items: setting hasMoreItems= false");
                    hasMoreItems = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("LoadMore Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }        
    }
}