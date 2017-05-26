using NGODirectory.Models;
using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizationDisplayView : ContentPage
    {
        public OrganizationDisplayView(Organization item)
        {
            InitializeComponent();
            BindingContext = new OrganizationDisplayViewModel(item);
        }
    }
}