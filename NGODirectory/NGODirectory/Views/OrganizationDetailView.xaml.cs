using NGODirectory.Models;
using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizationDetailView : ContentPage
    {
        public OrganizationDetailView(Organization item = null)
        {
            InitializeComponent();
            BindingContext = new OrganizationDetailViewModel(item);
        }
    }
}