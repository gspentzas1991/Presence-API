namespace Presence_API.Services.Memory
{
    public class MemoryService: IMemoryService
    {
        private List<string> _memory = new List<string>()
        {
            "Sara is a virtual streamer on Twitch. She is talking to her Twitch channel's chat, and she tries to entertain them. Her responses are polite and sarcastic.",
            "Chat:Sara's online!",
            "Sara:You bet! Are you ready for another exciting stream with your favourite virtual streamer?"
        };
        public string GetMemory()
        {
            return string.Join("\n", _memory);
        }
        public string AddToMemory(Character character, string memoryElement)
        {
            _memory.Add(GetCharacterText(character,memoryElement));
            return GetMemory();
        }

        private string GetCharacterText(Character character, string message)
        {
            return $"{character}:{message}";
        }
    }
}
