using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncementsListView : ContentPage
    {
        public AnnouncementsListView()
        {
            InitializeComponent();
            BindingContext = new AnnouncementsListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as AnnouncementsListViewModel;
            if (viewModel != null) viewModel.OnAppearing(null);
        }
    }
}