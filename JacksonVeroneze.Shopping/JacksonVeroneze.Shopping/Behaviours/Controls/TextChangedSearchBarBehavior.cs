using Xamarin.Forms;

namespace JacksonVeroneze.Shopping.Behaviours.Controls
{
    class TextChangedSearchBarBehavior : Behavior<SearchBar>
    {
        //
        // Summary:
        //     /// Method responsible for adding the validation method to the event. ///
        //
        // Parameters:
        //   bindable:
        //     The bindable param.
        //
        protected override void OnAttachedTo(SearchBar bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnSearchBarTextChanged;
        }

        //
        // Summary:
        //     /// Method responsible for removing the validation method from the event. ///
        //
        // Parameters:
        //   bindable:
        //     The bindable param.
        //
        protected override void OnDetachingFrom(SearchBar bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnSearchBarTextChanged;
        }

        //
        // Summary:
        //     /// Method responsible for validating / formatting the value. ///
        //
        // Parameters:
        //   sender:
        //     The sender param.
        //
        //   args:
        //     The args param.
        //
        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs args)
            => ((SearchBar)sender).SearchCommand?.Execute(args.NewTextValue);
    }
}