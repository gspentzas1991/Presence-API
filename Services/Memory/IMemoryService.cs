namespace Presence_API.Services.Memory
{
    public interface IMemoryService
    {
        string GetMemory();
        string AddToMemory(string chatRole, string memoryElement);
    }
}
