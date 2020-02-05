using FormsChatbot.Helpers;

namespace FormsChatbot
{
    public class AWSOptions : IAWSOptions
    {
        private AWSOptions() { }

        public string CognitoPoolID => Secrets.CognitoPoolID;

        public string LexBotName => Secrets.LexBotName;

        public string LexRole => Secrets.LexRole;

        public string LexBotAlias => Secrets.LexBotAlias;

        public string BotRegion => Secrets.BotRegion;
    }
}
