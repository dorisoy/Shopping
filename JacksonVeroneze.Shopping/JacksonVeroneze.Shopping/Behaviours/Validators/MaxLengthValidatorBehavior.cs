using Xamarin.Forms;

namespace JacksonVeroneze.Shopping.Behaviours.Validators
{
    public class MaxLengthValidatorBehavior : Behavior<Entry>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.NewTextValue))
                return;

            Entry entry = (Entry)sender;

            string entryText = entry.Text;

            if (entryText.Length > MaxLength)
            {
                entryText = entryText.Remove(entryText.Length - 1);

                entry.Text = entryText;
            }
        }
    }
}