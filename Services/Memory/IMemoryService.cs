namespace Presence_API.Services.Memory
{
    public enum Character
    {
        None = 0,
        Chat = 1,
        Sara = 2
    }

    public interface IMemoryService
    {
        string GetMemory();
        string AddToMemory(Character character, string memoryElement);
    }
}
