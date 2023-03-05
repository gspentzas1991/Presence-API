using Presence_API.Services.Memory;
using Presence_API.Services.TextToSpeech;
using OpenAIApi.Services;

namespace Presence_API.Middleware.Response
{
    public class ResponseMiddleware : IResponseMiddleware
    {
        private readonly int _maxChatPromptLength = 120;
        private readonly ILogger<ResponseMiddleware> _logger;
        private readonly IAIService _aiService;
        private readonly IMemoryService _memoryService;
        private readonly ITextToSpeechService _textToSpeechService;

        public ResponseMiddleware(ILogger<ResponseMiddleware> logger, IAIService aIService, IMemoryService memoryService, ITextToSpeechService textToSpeechService)
        {
            _logger = logger;
            _aiService = aIService;
            _memoryService = memoryService;
            _textToSpeechService = textToSpeechService;
        }

        public async Task<string> GetResponseInMemoryAsync(string chatPrompt)
        {
            chatPrompt = TruncatePrompt(chatPrompt);
            //Ask the filter if the question has bad words
            //var prompt = _memoryService.AddToMemory(ChatRole.user, chatPrompt);
            var response = await GetResponseAsync(chatPrompt);
            //Ask the filter if the question had bad words
            //var completeMemory = _memoryService.AddToMemory(ChatRole.assistant, response);
            return response;
        }

        public async Task<string> GetResponseAsync(string chatPrompt)
        {
            var response = await _aiService.GetCompletionResponseAsync(chatPrompt);
            var firstTextResponse = response.choices.FirstOrDefault()?.text;
            Console.WriteLine(firstTextResponse);
            _textToSpeechService.TalkAsync(firstTextResponse);
            //TODO: Send the response to the TTS service, and notify the Vtuber model to start animating
            return firstTextResponse;
        }

        /// <summary>
        /// If the prompt is bigger than the _maxChatPromptLength, it's truncated to that size
        /// </summary>
        private string TruncatePrompt(string prompt)
        {
            if (string.IsNullOrEmpty(prompt)) return prompt;
            return prompt.Length <= _maxChatPromptLength ? prompt : prompt.Substring(0, _maxChatPromptLength);
        }
    }
}
