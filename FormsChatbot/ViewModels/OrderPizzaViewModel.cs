using System;
using System.Threading.Tasks;
using Amazon.Lex;
using FormsChatbot.Events;
using FormsChatbot.Models;
using FormsChatbot.Services;
using MvvmHelpers;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials.Interfaces;

namespace FormsChatbot.ViewModels
{
    public class OrderPizzaViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAWSLexService _lexService;
        private readonly IPreferences _preferences;

        private string _currentUserId;

        public OrderPizzaViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            ILogger logger,
            IEventAggregator eventAggregator,
            IAWSLexService lexService,
            IPreferences preferences)
            : base(navigationService, pageDialogService, logger)
        {
            _eventAggregator = eventAggregator;
            _lexService = lexService;
            _preferences = preferences;

            Title = "Order Pizza";

            Messages = new ObservableRangeCollection<ChatBotMessage>();

            SendCommand = new DelegateCommand(async () => await ExecuteSendCommandAsync());

            _currentUserId = GetUserId();
        }

        public ObservableRangeCollection<ChatBotMessage> Messages { get; }

        public DelegateCommand SendCommand { get; }

        public string OutGoingText { get; set; } = string.Empty;

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var response = await _lexService.PutSessionAsync(_currentUserId, new Amazon.Lex.Model.DialogAction { IntentName = "Welcome", Type = DialogActionType.Delegate });
            var botMessage = ChatBotMessage.Create(1, MessageType.LexMessage, response.Message, DateTime.Now);
            AddAndScrollToItem(botMessage);

            base.OnNavigatedTo(parameters);
        }

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

        private string GetUserId()
        {
            var userId = _preferences.Get("user_id", string.Empty);
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = Guid.NewGuid().ToString();
                _preferences.Set("user_id", userId);
            }

            return userId;
        }
    }
}
