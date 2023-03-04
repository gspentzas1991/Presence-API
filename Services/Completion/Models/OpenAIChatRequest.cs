namespace Presence_API.Services.Completion.Models
{
    public enum ChatRole
    {
        none = 0,
        user = 1,
        assistant = 2
    }
    public class OpenAIChatRequest
    {
        public string model { get; set; }
        public List<OpenAIChatRequestMessage> messages { get; set; }
    }

    public class OpenAIChatRequestMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
