using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JacksonVeroneze.Shopping.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HasErrorControl : ContentView
    {
        // Prop: Title
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
                                                                 propertyName: "Title",
                                                                 returnType: typeof(string),
                                                                 declaringType: typeof(HasErrorControl),
                                                                 defaultValue: "Ocorreu um erro, tente novamente.",
                                                                 defaultBindingMode: BindingMode.TwoWay,
                                                                 propertyChanged: TitlePropertyChanged);

        public string Title
        {
            get { return GetValue(TitleProperty).ToString(); }
            set { SetValue(TitleProperty, value); }
        }

        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HasErrorControl control = bindable as HasErrorControl;

            control.title.Text = newValue.ToString();
        }

        //
        // Summary:
        //     /// Method responsible for initializing the control. ///
        //
        public HasErrorControl()
            => InitializeComponent();
    }
}