using DryIoc;
using FormsChatbot.Services;
using FormsChatbot.ViewModels;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;

namespace FormsChatbot
{
    public partial class App : PrismApplication
    {
        public App()
        { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var result = await NavigationService.NavigateAsync("NavigationPage/MainPage");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAWSOptions, AWSOptions>();
            containerRegistry.Register<IAWSLexService, AWSLexService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }
    }
}
