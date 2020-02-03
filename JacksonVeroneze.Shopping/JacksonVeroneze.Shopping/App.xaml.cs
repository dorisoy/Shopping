using JacksonVeroneze.Shopping.IoC;
using JacksonVeroneze.Shopping.ViewModels;
using JacksonVeroneze.Shopping.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace JacksonVeroneze.Shopping
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=17c08802-7bfc-441d-8eeb-ce5771c7fc23;" +
                  "ios=0b6e4b65-106b-4453-83ba-539cb9280365",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<CartPage, CartPageViewModel>();

            InjectorBootStrapper.RegisterTypes(containerRegistry);
            containerRegistry.RegisterForNavigation<CheckoutPage, CheckoutPageViewModel>();
        }
    }
}