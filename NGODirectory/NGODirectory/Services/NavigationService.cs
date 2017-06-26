using NGODirectory.Pages;
using NGODirectory.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NGODirectory.Services
{
    public class NavigationService
    {
        private static NavigationService _instance;
        private IDictionary<Type, Type> viewModelRouting = new Dictionary<Type, Type>()
        {
            { typeof(AnnouncementDisplayViewModel), typeof(AnnouncementDisplayPage) },
            { typeof(AnnouncementEditViewModel), typeof(AnnouncementEditPage) },
            { typeof(OrganizationDisplayViewModel), typeof(OrganizationDisplayPage) },
            { typeof(OrganizationEditViewModel), typeof(OrganizationEditPage) }
        };
        
        public static NavigationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NavigationService();
                }

                return _instance;
            }
        }

        public bool MenuIsPresented
        {
            get
            {
                return ((MasterDetailPage)(Application.Current.MainPage)).IsPresented;
            }
            set
            {
                ((MasterDetailPage)(Application.Current.MainPage)).IsPresented = value;
            }
        }

        public async Task NavigateTo<TDestinationViewModel>(object navigationContext = null)
        {
            Type pageType = viewModelRouting[typeof(TDestinationViewModel)];
            var page = Activator.CreateInstance(pageType, navigationContext) as Page;

            if (page != null)
                await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(page);
        }

        public async Task NavigateBack()
        {
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync();
        }

        public async Task GoToHomePage()
        {
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopToRootAsync();
        }
    }
}
