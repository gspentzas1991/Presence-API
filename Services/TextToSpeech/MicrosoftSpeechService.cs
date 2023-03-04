using Microsoft.CognitiveServices.Speech;

namespace Presence_API.Services.TextToSpeech
{
    public class MicrosoftSpeechService: ITextToSpeechService
    {
        private string _azureTTSSubscriptionKey;
        private string _azureTTSSRegion;
        public MicrosoftSpeechService(IConfiguration configuration)
        {
            _azureTTSSubscriptionKey = configuration["AzureTTS:AzureTTSSubscriptionKey"];
            _azureTTSSRegion = configuration["AzureTTS:AzureTTSSRegion"];
        }

        public async Task TalkAsync(string speechPrompt)
        {
            var config = SpeechConfig.FromSubscription(_azureTTSSubscriptionKey, _azureTTSSRegion);
            config.SpeechSynthesisVoiceName = "en-US-SaraNeural";
            using var synthesizer = new SpeechSynthesizer(config);

            //An ssml string, containing the speechPrompt
            var result = await synthesizer.SpeakSsmlAsync($"<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xmlns:emo=\"http://www.w3.org/2009/10/emotionml\" version=\"1.0\" xml:lang=\"en-US\"><voice name=\"en-US-SaraNeural\"><prosody rate=\"10%\" pitch=\"10%\">{speechPrompt}</prosody></voice></speak>");
        }
    }
}
