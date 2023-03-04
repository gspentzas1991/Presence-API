namespace Presence_API.Services.TextToSpeech
{
    public interface ITextToSpeechService
    {
        Task TalkAsync(string speechPrompt);
    }
}
