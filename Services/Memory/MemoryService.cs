namespace Presence_API.Services.Memory
{
    public class MemoryService: IMemoryService
    {
        private IEnumerable<string> _constantMemory = new string[]
        {
            "Sara is a virtual streamer on Twitch. She is talking to her Twitch channel's chat, and she tries to entertain them. Her responses are polite and sarcastic.",
            "Chat:Sara's online!",
            "Sara:You bet! Are you ready for another exciting stream with your favourite virtual streamer?"
        };
        private List<string> _mutableMemory = new List<string>()
        {
        };
        private int _mutableMemoryMaxSize = 10;
        public string GetMemory()
        {
            var memory = _constantMemory.Concat(_mutableMemory);
            return string.Join("\n", memory);
        }
        public string AddToMemory(Character character, string memoryElement)
        {
            while(_mutableMemory.Count >= _mutableMemoryMaxSize)
            {
                RemoveOldestMemoryPrompt();
            }
            _mutableMemory.Add(GetCharacterText(character,memoryElement));
            return GetMemory();
        }

        private string GetCharacterText(Character character, string message)
        {
            return $"{character}:{message}";
        }

        /// <summary>
        /// Removes the oldest memory prompt, and its response. 
        /// </summary>
        private void RemoveOldestMemoryPrompt()
        {
            if (_mutableMemory.Count == 0)
            {
                return;
            }
            _mutableMemory.RemoveAt(0);
            //After removing the oldest memory, if the next memory is by the AI, it's a response and should also be removed
            var oldestMemoryIsResponse = _mutableMemory.Count>0 &&_mutableMemory[0].StartsWith(Character.Sara.ToString());
            if (oldestMemoryIsResponse)
            {
                _mutableMemory.RemoveAt(0);
            }
        }
    }
}
