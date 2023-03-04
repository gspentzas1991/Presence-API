using Presence_API.Controllers;
using Presence_API.Models;
using Presence_API.Services.Memory;
using System;
using System.Net.Http.Headers;

namespace Presence_API.Services.Completion
{
    public class OpenAICompletionService : ICompletionService
    {
        private string[] _basePrompt = {
            "Sara is a virtual streamer on Twitch. She is talking to her Twitch channel's chat, and she tries to entertain them. Her responses are polite and sarcastic.",
            "Chat:Sara's online!",
            "Sara:You bet! Are you ready for another exciting stream with your favourite virtual streamer?"
        };
        private readonly string? _openAIApiKey;
        private readonly ILogger<OpenAICompletionService> _logger;

        public OpenAICompletionService(ILogger<OpenAICompletionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _openAIApiKey = configuration["OpenAIApiKey"];
        }
        public async Task<OpenAIApiResponse> GetPromptCompletionAsync(string prompt)
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
            var basePrompt = string.Join("\n", _basePrompt);
            prompt += $"\n{Character.Sara}:";
            var data = new
            {
                model = "text-davinci-003",
                prompt = basePrompt +"\n"+ prompt,
                temperature = 0.9,
                max_tokens = 150,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0.6,
                stop = new string[] { "\n", "\nSara:", "\nChat:" }
            };
            Console.WriteLine(data.prompt);
            return data;
        }
    }
}
