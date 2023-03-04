using Presence_API.Controllers;
using Presence_API.Services.Completion.Models;
using Presence_API.Services.Memory;
using System;
using System.Net.Http.Headers;
using TwitchLib.Api.Helix.Models.Charity.GetCharityCampaign;

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
            _openAIApiKey = configuration["OpenAI:ApiKey"];
        }
        public async Task<OpenAIApiCompletionResponse> GetPromptCompletionAsync(string prompt)
        {
            using (var client = GetOpenApiHttpClient())
            {
                var httpResponse = await client.PostAsJsonAsync("https://api.openai.com/v1/completions", GetOpenAICompletionObject(prompt));
                return await httpResponse.Content.ReadAsAsync<OpenAIApiCompletionResponse>();
            }
        }
        public async Task<OpenAIApiCompletionResponse> GetChatResponseAsync(string prompt)
        {
            return null;
        }

        public async Task<OpenAIApiCompletionResponse> FilterPrompt(string prompt)
        {
            return null;
        }

        private HttpClient GetOpenApiHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAIApiKey);
            return client;
        }

        private Object GetOpenAICompletionObject(string prompt)
        {
            var basePrompt = string.Join("\n", _basePrompt);
            prompt += $"\n{ChatRole.assistant}:";
            var data = new OpenAICompletionRequest()
            {
                model = "text-davinci-003",
                prompt = basePrompt +"\n"+ prompt,
                temperature = 0.9,
                max_tokens = 150,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0.6,
                stop = new List<string>() { "\n", "\nSara:", "\nChat:" }
            };
            Console.WriteLine(data.prompt);
            return data;
        }

        private Object GetOpenAIChatObject(string chatPrompt)
        {
            var basePrompt = string.Join("\n", _basePrompt);
            chatPrompt += $"\n{ChatRole.assistant}:";
            var data = new OpenAIChatRequest()
            {
                model= "gpt-3.5-turbo",
                messages = new List<OpenAIChatRequestMessage>() { }
            };
            Console.WriteLine(data.messages);
            return data;
        }
    }
}
