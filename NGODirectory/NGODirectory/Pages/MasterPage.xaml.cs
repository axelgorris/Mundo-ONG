using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();
            BindingContext = new MasterModel();
        }
    }
}