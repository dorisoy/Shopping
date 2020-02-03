using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Droid.Services;
using Prism;
using Prism.Ioc;

namespace JacksonVeroneze.Shopping.Droid
{
    [Activity(Label = "JacksonVeroneze.Shopping", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            UserDialogs.Init(this);

            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDbConnectionProvider, DbConnectionProvider>();
        }
    }
}