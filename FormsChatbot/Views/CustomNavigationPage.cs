using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace FormsChatbot.Views
{
    public partial class CustomNavigationPage : Xamarin.Forms.NavigationPage
    {
        public CustomNavigationPage()
        {
            On<iOS>().SetPrefersLargeTitles(true);

            BarBackgroundColor = (Xamarin.Forms.Color)Xamarin.Forms.Application.Current.Resources["NavigationPrimary"];
            BarTextColor = Xamarin.Forms.Color.White;
        }
    }
}
