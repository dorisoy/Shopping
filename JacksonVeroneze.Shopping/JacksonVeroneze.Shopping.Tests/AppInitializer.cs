using Xamarin.UITest;

namespace JacksonVeroneze.Shopping.Tests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                 .Android
                 .InstalledApp("com.jacksonveroneze.shopping")
                 .StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}