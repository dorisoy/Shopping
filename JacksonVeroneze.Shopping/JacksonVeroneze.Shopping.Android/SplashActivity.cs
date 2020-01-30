using Android.App;
using Android.OS;

namespace JacksonVeroneze.Shopping.Droid
{
    [Activity(Icon = "@mipmap/ic_launcher", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MainActivity));

            Finish();

            OverridePendingTransition(0, 0);
        }
    }
}