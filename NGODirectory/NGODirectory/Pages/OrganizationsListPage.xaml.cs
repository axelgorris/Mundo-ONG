using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizationsListPage : ContentPage
    {
        public OrganizationsListPage()
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