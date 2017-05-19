using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryView : ContentPage
    {
        public EntryView()
        {
            InitializeComponent();
            BindingContext = new EntryPageViewModel();
        }
    }
}