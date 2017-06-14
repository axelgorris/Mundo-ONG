using Xamarin.Forms;

namespace NGODirectory.Behaviors
{
    public class EditorMaxLengthValidatorBehavior : Behavior<Editor>
    {
        public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create("MaxLength", typeof(int), typeof(EntryMaxLengthValidatorBehavior), 0);

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        
        protected override void OnAttachedTo(Editor bindable)
        {
            bindable.TextChanged += bindable_TextChanged;
        }

        private void bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= MaxLength)
                ((Editor)sender).Text = e.NewTextValue.Substring(0, MaxLength);
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            bindable.TextChanged -= bindable_TextChanged;
        }
    }
}
