namespace FormsChatbot
{
    public interface IAWSOptions
    {
        string CognitoPoolID { get; }

        string LexBotName { get; }

        string LexRole { get; }

        string LexBotAlias { get; }

        string BotRegion { get; }
    }
}
