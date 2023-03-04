using Presence_API.Controllers;
using Presence_API.Services.Completion;
using Presence_API.Services.Completion.Models;
using Presence_API.Services.Memory;
using Presence_API.Services.TextToSpeech;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presence_API.Middleware.Response
{
    public class ResponseMiddleware : IResponseMiddleware
    {
        private readonly int _maxChatPromptLength = 120;
        private readonly ILogger<ResponseMiddleware> _logger;
        private readonly ICompletionService _completionService;
        private readonly IMemoryService _memoryService;
        private readonly ITextToSpeechService _textToSpeechService;

        public ResponseMiddleware(ILogger<ResponseMiddleware> logger, ICompletionService completionService, IMemoryService memoryService, ITextToSpeechService textToSpeechService)
        {
            _logger = logger;
            _completionService = completionService;
            _memoryService = memoryService;
            _textToSpeechService = textToSpeechService;
        }

        public async Task<string> GetResponseInMemoryAsync(string chatPrompt)
        {
            chatPrompt = TruncatePrompt(chatPrompt);
            //Ask the filter if the question has bad words
            var prompt = _memoryService.AddToMemory(ChatRole.user, chatPrompt);
            var response = await GetResponseAsync(prompt);
            //Ask the filter if the question had bad words
            var completeMemory = _memoryService.AddToMemory(ChatRole.assistant, response);
            return response;
        }

        public async Task<string> GetResponseAsync(string chatPrompt)
        {
            var response = await _completionService.GetPromptCompletionAsync(chatPrompt);
            var firstTextResponse = response.Choices.FirstOrDefault()?.Text;
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
