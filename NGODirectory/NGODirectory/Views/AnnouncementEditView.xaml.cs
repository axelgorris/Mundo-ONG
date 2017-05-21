using NGODirectory.Models;
using NGODirectory.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncementEditView : ContentPage
    {
        public AnnouncementEditView(Announcement item = null)
        {
            InitializeComponent();
            BindingContext = new AnnouncementEditViewModel(item);
        }
    }
}