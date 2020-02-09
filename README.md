# FormsChatbot
Xamarin.Forms App using AWS Amazon Lex Bot Service

## Builds

Branch|Platform|Status
------|------|------
Master|iOS|[![Build status]()](https://appcenter.ms)

## Getting the application to build locally

Create a file named secrets.json in the FormsChatbot project, and update it to look like the following:

```json
{
    "AppCenter_iOS_Secret": "{Your AppCenter iOS Application Secret HERE}",
    "AppCenter_Android_Secret": "{Your AppCenter Android Application Secret HERE}",
    "CognitoPoolID": "{Your Cognito IdentityPool Id HERE}",
    "LexBotName": "{Your AWS Lex Bot Name HERE}",
    "LexRole": "{Your AWS Lex Bot Role HERE}",
    "LexBotAlias": "{Your AWS Lex Bot Alias HERE}",
    "BotRegion": "{Your AWS Lex Bot Region HERE}"
}
```
