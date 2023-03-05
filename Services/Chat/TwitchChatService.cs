using Presence_API.Middleware.Response;
using TwitchLib.Api.ThirdParty.UsernameChange;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub.Models.Responses;

namespace Presence_API.Services.Chat
{
    /// <summary>
    /// An implementation of ChatService for Twitch. Receives twitch chats, subscription notices and chat bits
    /// </summary>
    public class TwitchChatService: IChatService
    {
        private static TwitchClient _twitchClient;
        private static TwitchPubSub _twitchPubsubClient;
        private readonly IResponseMiddleware _response;
        private readonly ILogger<TwitchChatService> _logger;

        public TwitchChatService(ILogger<TwitchChatService> logger, IConfiguration configuration, IResponseMiddleware responseMiddleware)
        {
            _logger = logger;
            _response = responseMiddleware;
            var twitchUsername = configuration["Twitch:Username"];
            var twitchOAuthKey = configuration["Twitch:OAuthKey"];
            InitializeTwitchClient(twitchUsername, twitchOAuthKey);
            InitializePubSubClient(twitchUsername);
        }

        private void InitializeTwitchClient(string twitchUsername, string twitchOAuthKey)
        {
            ConnectionCredentials credentials = new ConnectionCredentials(twitchUsername, twitchOAuthKey);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            _twitchClient = new TwitchClient(customClient);
            _twitchClient.OnMessageReceived += Client_OnMessageReceived;
            _twitchClient.OnNewSubscriber += Client_OnNewSubscriber;
            _twitchClient.Initialize(credentials, twitchUsername);
            _twitchClient.Connect();
        }

        private void InitializePubSubClient(string twitchUsername)
        {
            _twitchPubsubClient = new TwitchPubSub();
            _twitchPubsubClient.OnBitsReceivedV2 += Client_ReceivedBits;
            _twitchPubsubClient.ListenToBitsEventsV2(twitchUsername);
            _twitchPubsubClient.Connect();
        }

        /// <summary>
        /// Event that fires when someone sends a chat on the channel
        /// </summary>
        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            //TODO: add a banList for words
            if (e.ChatMessage.Message.Contains("badword"))
                _twitchClient.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");
            _response.GetResponseInMemoryAsync(e.ChatMessage.Message);
        }

        /// <summary>
        /// Event that fires when someone subscribes to the channel
        /// </summary>
        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            var resubMessage = $"user {e.Subscriber.DisplayName} subbed to the channel with the message {e.Subscriber.ResubMessage}";
            _response.GetResponseAsync(resubMessage);
        }

        /// <summary>
        /// Event that fires when someone donates bits to the channel
        /// </summary>
        private void Client_ReceivedBits(object sender, OnBitsReceivedV2Args e)
        {
            var bitDonationmessage = $"user {e.UserName} donated ${e.BitsUsed} bits to the channel, with the message {e.ChatMessage}";
            _response.GetResponseAsync(bitDonationmessage);
        }

        public void TestNewSubscriber()
        {
            var subscriber = new Subscriber(null, null, null, System.Drawing.Color.Red, displayName: "Batman34", null, null, null, null, null, "10", null, false, null, "I love you!", SubscriptionPlan.Prime, null, null, userId: "myId", false, false, false, false, null, UserType.Viewer, null, null);
            //subscriber
        }

        public void TestNewBitDonation()
        {
            var bit = new OnBitsReceivedV2Args();
            //bit
        }
    }
}
