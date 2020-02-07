using System;
using Humanizer;

namespace FormsChatbot.Models
{
    public class ChatBotMessage
    {
        //0 -> UserMessage 
        //1-> BotMessage
        public int ID { get; private set; }

        public MessageType Type { get; private set; }

        public string Message { get; private set; }

        public DateTimeOffset MessageDateTime { get; private set; }

        public string MessageDateTimeHumanized => MessageDateTime.Humanize();

        public static ChatBotMessage Create(int id, MessageType messageType, string message, DateTimeOffset dateTime)
        {
            return new ChatBotMessage
            {
                ID = id,
                Type = messageType,
                Message = message,
                MessageDateTime = dateTime
            };
        }
    }

    public enum MessageType
    {
        UserMessage,
        LexMessage
    }
}
