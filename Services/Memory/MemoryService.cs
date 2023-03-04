using Presence_API.Services.Completion.Models;

namespace Presence_API.Services.Memory
{
    public class MemoryService: IMemoryService
    {
        private List<string> _memory = new List<string>();
        private int _memoryMaxSize = 10;

        public string GetMemory()
        {
            return string.Join("\n", _memory);
        }

        public string AddToMemory(ChatRole chatRole, string memoryElement)
        {
            while(_memory.Count >= _memoryMaxSize)
            {
                RemoveOldestMemoryPrompt();
            }
            _memory.Add(GetCharacterText(chatRole,memoryElement));
            return GetMemory();
        }

        private string GetCharacterText(ChatRole character, string message)
        {
            return $"{character}:{message}";
        }

        /// <summary>
        /// Removes the oldest memory prompt, and its response. 
        /// </summary>
        private void RemoveOldestMemoryPrompt()
        {
            if (_memory.Count == 0)
            {
                return;
            }
            _memory.RemoveAt(0);
            //After removing the oldest memory, if the next memory is by the AI, it's a response and should also be removed
            var oldestMemoryIsResponse = _memory.Count>0 &&_memory[0].StartsWith(ChatRole.assistant.ToString());
            if (oldestMemoryIsResponse)
            {
                _memory.RemoveAt(0);
            }
        }
    }
}
