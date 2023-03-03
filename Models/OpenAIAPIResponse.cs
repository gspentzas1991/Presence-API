namespace Presence_API.Models
{
    public class OpenAIApiResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public string Created { get; set; }
        public string Model { get; set; }
        public IEnumerable<OpenAIChoice> Choices { get; set; }
        public OpenAIUsage Usage { get; set; }
    }

    public class OpenAIChoice
    {
        public string Text { get; set; }
        public int Index { get; set; }
        public string LogProbs { get; set; }
        public string Finish_Reason { get; set; }
    }

    public class OpenAIUsage
    {
        public int Prompt_Tokens { get; set; }
        public int Completion_Tokens { get; set; }
        public int Total_Tokens { get; set; }
    }
}
