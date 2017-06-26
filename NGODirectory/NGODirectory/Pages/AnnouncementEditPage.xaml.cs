using NGODirectory.Models;
using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncementEditPage : ContentPage
    {
        public AnnouncementEditPage(Announcement item = null)
        {
            InitializeComponent();
            BindingContext = new AnnouncementEditViewModel(item);
        }
    }
}