using NGODirectory.Models;
using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncementDisplayPage : ContentPage
    {
        public AnnouncementDisplayPage(Announcement item)
        {
            InitializeComponent();
            BindingContext = new AnnouncementDisplayViewModel(item);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as AnnouncementDisplayViewModel;
            if (viewModel != null) viewModel.OnAppearing(null);
        }
    }
}