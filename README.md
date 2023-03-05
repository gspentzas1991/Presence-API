# Presence API
A Web API project that can be used to receive responses from text prompts. It uses the OpenAI API to create the responses, and saves the last 10 pieces of conversation to memory, in order for the chat to make sense.

# Requirements
Requires to also pull the [ChatGPT-API project](https://github.com/gspentzas1991/ChatGPT-API)

# Usage
In order to register your OpenAI API Key, you need to create a User Secret file with your API key
```
{
  "OpenAI": {
    "ApiKey": "YourApiKey"
  },
  "Twitch": {
    "Username": "YourTwitchChannelUsername",
    "OAuthKey": "YourTwitchChannelOAuthKey"
  },
  "AzureTTS": {
    "AzureTTSSubscriptionKey": "YourAzureTTSSubscriptionKey",
    "AzureTTSSRegion": "YourAzureTTSRegion"
  }
}
```
