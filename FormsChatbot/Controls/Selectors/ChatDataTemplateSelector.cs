using FormsChatbot.Controls.Cells;
using FormsChatbot.Models;
using Xamarin.Forms;

namespace FormsChatbot.Controls.Selectors
{
    public class ChatDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _incomingDataTemplate;
        private readonly DataTemplate _outgoingDataTemplate;

        public ChatDataTemplateSelector()
        {
            _incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            _outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is ChatBotMessage messageVm))
                return null;

            return messageVm.Type == MessageType.LexMessage ? _incomingDataTemplate : _outgoingDataTemplate;
        }
    }
}
