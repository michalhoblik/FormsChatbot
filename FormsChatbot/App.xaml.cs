using System.Threading.Tasks;
using DryIoc;
using FormsChatbot.Services;
using FormsChatbot.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.AppCenter;
using Xamarin.Forms;

namespace FormsChatbot
{
    public partial class App : PrismApplication
    {
        /* 
        * NOTE: 
        * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
        * This imposes a limitation in which the App class must have a default constructor. 
        * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
        */
        public App()
            : this(null)
        { }

        public App(IPlatformInitializer initializer = null)
             : base(initializer)
        { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            LogUnobservedTaskExceptions();

            AppCenter.Start(AppConstants.AppCenterStart, typeof(Analytics), typeof(Crashes));

            Container.Resolve<ILogger>().Log("Started App Center");

            Resources.Add("eventAggregator", Container.Resolve<IEventAggregator>());
            var result = await NavigationService.NavigateAsync("NavigationPage/MainPage");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register Loggers
            var logger = new AppCenterLogger();
            containerRegistry.RegisterInstance<ILogger>(logger);
            containerRegistry.RegisterInstance<ILoggerFacade>(logger);

            containerRegistry.RegisterSingleton<IAWSOptions, AWSOptions>();
            containerRegistry.Register<IAWSLexService, AWSLexService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

        private void LogUnobservedTaskExceptions()
        {
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Container.Resolve<ILogger>().Report(e.Exception);
            };
        }
    }
}
