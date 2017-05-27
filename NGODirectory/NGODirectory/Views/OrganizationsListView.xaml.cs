using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizationsListView : ContentPage
    {
        public OrganizationsListView()
        {
            InitializeComponent();
            BindingContext = new OrganizationsListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as OrganizationsListViewModel;
            if (viewModel != null) viewModel.OnAppearing(null);
        }
    }
}