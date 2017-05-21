using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
            BindingContext = new SettingsPageViewModel();
        }
    }
}