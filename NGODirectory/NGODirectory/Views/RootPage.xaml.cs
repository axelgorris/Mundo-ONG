using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;

            //Master = new SettingsView();
            //Detail = new CustomNavigationPage(new BarPage());
            //MD.Detail = Detail;
        }
    }
}