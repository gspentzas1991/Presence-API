using Presence_API.Controllers;
using Presence_API.Services.Completion;
using Presence_API.Services.Memory;

namespace Presence_API.Middleware.Response
{
    public class ResponseMiddleware : IResponseMiddleware
    {
        private readonly int _maxChatPromptLength = 120;
        private readonly ILogger<ResponseMiddleware> _logger;
        private readonly ICompletionService _completionService;
        private readonly IMemoryService _memoryService;

        public ResponseMiddleware(ILogger<ResponseMiddleware> logger, ICompletionService completionService, IMemoryService memoryService)
        {
            _logger = logger;
            _completionService = completionService;
            _memoryService = memoryService;
        }

        public async Task<string> GetResponseInMemoryAsync(string chatPrompt)
        {
            chatPrompt = TruncatePrompt(chatPrompt);
            var prompt = _memoryService.AddToMemory(Character.Chat, chatPrompt);
            var response = await GetResponseAsync(prompt);
            _memoryService.AddToMemory(Character.Sara, response);
            return response;
        }

        public async Task<string> GetResponseAsync(string chatPrompt)
        {
            chatPrompt = TruncatePrompt(chatPrompt);
            var response = await _completionService.GetPromptCompletionAsync(chatPrompt);
            var firstTextResponse = response.Choices.FirstOrDefault()?.Text;
            //TODO: Send the response to the TTS service, and notify the Vtuber model to start animating
            Console.WriteLine(firstTextResponse);
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
