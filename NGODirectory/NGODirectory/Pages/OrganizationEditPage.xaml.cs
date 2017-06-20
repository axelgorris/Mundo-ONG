using NGODirectory.Models;
using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizationEditPage : ContentPage
    {
        public OrganizationEditPage(Organization item = null)
        {
            InitializeComponent();
            BindingContext = new OrganizationEditViewModel(item);
        }
    }
}