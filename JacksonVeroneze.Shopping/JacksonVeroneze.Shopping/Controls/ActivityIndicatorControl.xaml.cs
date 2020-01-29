using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JacksonVeroneze.Shopping.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityIndicatorControl : ContentView
    {
        // Prop: Title
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
                                                                 propertyName: "Title",
                                                                 returnType: typeof(string),
                                                                 declaringType: typeof(ActivityIndicatorControl),
                                                                 defaultValue: "Carregando",
                                                                 defaultBindingMode: BindingMode.TwoWay,
                                                                 propertyChanged: TitlePropertyChanged);

        public string Title
        {
            get { return GetValue(TitleProperty).ToString(); }
            set { SetValue(TitleProperty, value); }
        }

        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ActivityIndicatorControl control = bindable as ActivityIndicatorControl;

            control.title.Text = newValue.ToString();

            control.title.IsVisible = true;
        }

        //
        // Summary:
        //     /// Method responsible for initializing the control. ///
        //
        public ActivityIndicatorControl()
            => InitializeComponent();
    }
}