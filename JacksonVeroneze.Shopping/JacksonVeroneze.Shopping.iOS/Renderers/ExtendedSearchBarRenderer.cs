using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(JacksonVeroneze.Shopping.iOS.Renderers.ExtendedSearchBarRenderer))]

namespace JacksonVeroneze.Shopping.iOS.Renderers
{
    public class ExtendedSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0) && Control != null)
            {
                Control.SearchTextField.BackgroundColor = UIColor.FromRGB(255, 255, 255);
                Control.SearchBarStyle = UISearchBarStyle.Minimal;
            }

            Control.BarTintColor = UIColor.White;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);


            if (e.PropertyName == "Text")
            {
                Control.ShowsCancelButton = false;
            }
        }
    }
}