using Presence_API.Models;

namespace Presence_API.Services.Completion
{
    public interface ICompletionService
    {
        Task<OpenAIApiResponse> CompleteAsync(string prompt);
    }
}
