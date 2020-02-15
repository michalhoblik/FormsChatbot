using System.Threading.Tasks;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;

namespace FormsChatbot.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            ILogger logger) : base(navigationService, pageDialogService, logger)
        {
            Title = "Chatbot App";

            OpenOrderPizzaPageCommand = new DelegateCommand(async () => await ExecuteOpenOrderPizzaPageAsync());
        }

        public DelegateCommand OpenOrderPizzaPageCommand { get; }

        private async Task ExecuteOpenOrderPizzaPageAsync()
        {
            await NavigationService.NavigateAsync("OrderPizzaPage");
        }
    }
}
