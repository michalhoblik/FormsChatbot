using System;
using System.Threading.Tasks;
using FormsChatbot.Events;
using FormsChatbot.Models;
using FormsChatbot.Services;
using MvvmHelpers;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;

namespace FormsChatbot.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAWSLexService _lexService;
        private string _currentUserId;

        public MainPageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            ILogger logger,
            IEventAggregator eventAggregator,
            IAWSLexService lexService)
            : base(navigationService, pageDialogService, logger)
        {
            _eventAggregator = eventAggregator;
            _lexService = lexService;

            _currentUserId = Guid.NewGuid().ToString();

            Messages = new ObservableRangeCollection<ChatBotMessage>();

            SendCommand = new DelegateCommand(async () => await ExecuteSendCommandAsync());
        }

        public ObservableRangeCollection<ChatBotMessage> Messages { get; }

        public DelegateCommand SendCommand { get; }

        public string OutGoingText { get; set; } = string.Empty;

        private async Task ExecuteSendCommandAsync()
        {
            try
            {
                var userMessage = ChatBotMessage.Create(0, MessageType.UserMessage, OutGoingText, DateTime.Now);
                AddAndScrollToItem(userMessage);

                OutGoingText = string.Empty;

                var response = await _lexService.PostTextAsync(userMessage.Message, _currentUserId);

                var botMessage = ChatBotMessage.Create(1, MessageType.LexMessage, response.Message, DateTime.Now);
                AddAndScrollToItem(botMessage);
            }
            catch (Exception ex)
            {
                Logger.Report(ex);
                await PageDialogService.DisplayAlertAsync("Error", "An error occurred", "OK");
            }
        }

        private void AddAndScrollToItem(ChatBotMessage message)
        {
            Messages.Add(message);
            _eventAggregator.GetEvent<ScrollToChatBotMessageEvent>().Publish(message);
        }
    }
}
