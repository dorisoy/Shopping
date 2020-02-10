using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(JacksonVeroneze.Shopping.Droid.Renderers.ExtendedSearchBarRenderer))]

namespace JacksonVeroneze.Shopping.Droid.Renderers
{
    public class ExtendedSearchBarRenderer : SearchBarRenderer
    {
        public ExtendedSearchBarRenderer(Context context) : base(context: context) {  }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null) return;

            var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);

            var plate = Control.FindViewById(plateId);

            plate.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}