using NGODirectory.Models;
using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganizationEditView : ContentPage
    {
        public OrganizationEditView(Organization item = null)
        {
            InitializeComponent();
            BindingContext = new OrganizationEditViewModel(item);
        }
    }
}