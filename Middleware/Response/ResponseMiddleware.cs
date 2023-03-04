using Presence_API.Controllers;
using Presence_API.Services.Completion;
using Presence_API.Services.Memory;

namespace Presence_API.Middleware.Response
{
    public class ResponseMiddleware : IResponseMiddleware
    {
        private readonly ILogger<ResponseMiddleware> _logger;
        private readonly ICompletionService _completionService;
        private readonly IMemoryService _memoryService;

        public ResponseMiddleware(ILogger<ResponseMiddleware> logger, ICompletionService completionService, IMemoryService memoryService)
        {
            _logger = logger;
            _completionService = completionService;
            _memoryService = memoryService;
        }

        public async Task<string> GetResponseAsync(string chatPrompt)
        {
            var memory = _memoryService.AddToMemory(Character.Chat, chatPrompt);
            var response = await _completionService.GetPromptCompletionAsync(memory);
            var firstTextResponse = response.Choices.FirstOrDefault()?.Text;
            _memoryService.AddToMemory(Character.Sara, firstTextResponse);
            return firstTextResponse;
        }
    }
}
