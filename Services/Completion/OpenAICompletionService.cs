using Presence_API.Controllers;
using Presence_API.Models;
using Presence_API.Services.Memory;
using System.Net.Http.Headers;

namespace Presence_API.Services.Completion
{
    public class OpenAICompletionService: ICompletionService
    {
        private readonly string? _openAIApiKey;
        private readonly ILogger<OpenAICompletionService> _logger;

        public OpenAICompletionService(ILogger<OpenAICompletionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _openAIApiKey = configuration["OpenAIApiKey"];
        }
        public async Task<OpenAIApiResponse> CompleteAsync(string prompt)
        {
            using (var client = GetOpenApiHttpClient())
            {
                var httpResponse = await client.PostAsJsonAsync("https://api.openai.com/v1/completions", GetOpenAiCompletionObject(prompt));
                return await httpResponse.Content.ReadAsAsync<OpenAIApiResponse>();
            }
        }

        private HttpClient GetOpenApiHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAIApiKey);
            return client;
        }

        private Object GetOpenAiCompletionObject(string prompt)
        {
            prompt += $"{Character.Sara}:";
            var data = new
            {
                model = "text-davinci-003",
                prompt = prompt,
                temperature = 0.9,
                max_tokens = 150,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0.6,
                stop = new string[] { "\n", "\nSara:", "\nChat:" }
            };
            return data;
        }
    }
}
