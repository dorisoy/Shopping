using Prism;
using Prism.Ioc;
using JacksonVeroneze.Shopping.ViewModels;
using JacksonVeroneze.Shopping.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JacksonVeroneze.Shopping.IoC;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace JacksonVeroneze.Shopping
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<CartPage, CartPageViewModel>();

            InjectorBootStrapper.RegisterTypes(containerRegistry);
        }
    }
}
