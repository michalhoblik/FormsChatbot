using FormsChatbot.Models;
using Prism.Events;

namespace FormsChatbot.Events
{
    public class ScrollToChatBotMessageEvent : PubSubEvent<ChatBotMessage>
    {
    }
}
