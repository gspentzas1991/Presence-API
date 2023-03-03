using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Headers;
using System;
using Newtonsoft.Json;
using System.Text;
using Presence_API.Models;

namespace Presence_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : ControllerBase
    {
        private readonly ILogger<ResponseController> _logger;
        private readonly string? _openAIApiKey;

        public ResponseController(ILogger<ResponseController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _openAIApiKey = configuration["OpenAIApiKey"];
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAIApiKey);
                    var data = new
                    {
                        model = "text-davinci-003",
                        prompt = "Sara is a virtual streamer on Twitch. She is talking to her Twitch channel's chat, and she tries to entertain them. Her responses are polite and sarcastic.\nChat:Sara's online!\nSara:You bet! Are you ready for another exciting stream with your favourite virtual streamer?\nChat:Yeah!\nSara:",
                        temperature = 0.9,
                        max_tokens = 150,
                        top_p = 1,
                        frequency_penalty = 0,
                        presence_penalty = 0.6,
                        stop = new string[] { "\n", "\nSara:", "\nChat:" }
                    };
                    var httpResponse = await client.PostAsJsonAsync("https://api.openai.com/v1/completions", data);
                    var responseContent = await httpResponse.Content.ReadAsAsync<OpenAIApiResponse>();
                    var firstResponseText = responseContent.Choices.FirstOrDefault()?.Text;
                    return Ok(firstResponseText);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
