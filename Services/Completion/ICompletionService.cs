using Presence_API.Services.Completion.Models;

namespace Presence_API.Services.Completion
{
    public interface ICompletionService
    {
        Task<OpenAIApiCompletionResponse> GetPromptCompletionAsync(string prompt);
        Task<OpenAIApiCompletionResponse> GetChatResponseAsync(string prompt);
        Task<OpenAIApiCompletionResponse> FilterPrompt(string prompt);

    }
}
