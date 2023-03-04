namespace Presence_API.Services.Completion.Models
{
    public class OpenAICompletionRequest
    {
        public string model { get; set; }
        public string prompt { get; set; }
        public double temperature { get; set; }
        public int max_tokens { get; set; }
        public int top_p { get; set; }
        public int frequency_penalty { get; set; }
        public double presence_penalty { get; set; }
        public List<string> stop { get; set; }

    }
}
