using System;
using NGODirectory.Abstractions;
using NGODirectory.Helpers;
using Xamarin.Forms;

namespace NGODirectory.Controls
{
    public class ConditionalToolbarItem : ToolbarItem
    {
        //NOTE: Default value is true, because toolbar items are by default visible when added to the Toolbar
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ConditionalToolbarItem), true, BindingMode.TwoWay, null, OnIsVisibleChanged);

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public ContentPage ParentPage
        {
            get { return Parent as ContentPage; }
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var item = bindable as ConditionalToolbarItem;

            bool newVisible = (bool)newvalue;

            if (item.ParentPage == null)
                return;

            var items = item.ParentPage.ToolbarItems;

            Device.BeginInvokeOnMainThread(() =>
            {
                if (newVisible && !items.Contains(item))
                {
                    items.Add(item);
                }
                else if (!newVisible && items.Contains(item))
                {
                    items.Remove(item);
                }
            });
        }
    }
}
