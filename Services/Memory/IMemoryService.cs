using Presence_API.Services.Completion.Models;

namespace Presence_API.Services.Memory
{
    public interface IMemoryService
    {
        string GetMemory();
        string AddToMemory(ChatRole chatRole, string memoryElement);
    }
}
