using Microsoft.AspNetCore.Mvc;
using Presence_API.Middleware.Response;
using Presence_API.Services.Completion;
using Presence_API.Services.Memory;

namespace Presence_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : ControllerBase
    {
        private readonly ILogger<ResponseController> _logger;
        private readonly IResponseMiddleware _response;

        public ResponseController(ILogger<ResponseController> logger, IResponseMiddleware responseMiddleware)
        {
            _logger = logger;
            _response = responseMiddleware;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string chatPrompt)
        {
            try
            {
                var response = await _response.GetResponseAsync(chatPrompt);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
