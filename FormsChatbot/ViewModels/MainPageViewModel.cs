using System;
using System.Threading.Tasks;
using FormsChatbot.Services;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;

namespace FormsChatbot.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IPageLifecycleAware
    {
        private readonly IAWSLexService _lexService;
        private string _currentSessionId;

        public MainPageViewModel(
            INavigationService navigationService,
            IAWSLexService lexService)
            : base(navigationService)
        {
            _lexService = lexService;

            _currentSessionId = Guid.NewGuid().ToString();

            StartSessionCommand = new DelegateCommand(async () => await ExecuteStartSessionAsync());
        }

        public DelegateCommand StartSessionCommand { get; }

        public void OnAppearing()
        {
        }

        public void OnDisappearing()
        {
            
        }

        private async Task ExecuteStartSessionAsync()
        {
            try
            {
                var response = await _lexService.PutSessionAsync(_currentSessionId);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
