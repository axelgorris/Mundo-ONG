﻿using NGODirectory.Models;
using NGODirectory.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NGODirectory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnouncementDisplayView : ContentPage
    {
        public AnnouncementDisplayView(Announcement item)
        {
            InitializeComponent();
            BindingContext = new AnnouncementDisplayViewModel(item);
        }
    }
}