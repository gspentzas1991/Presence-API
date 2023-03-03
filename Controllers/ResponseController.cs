using Microsoft.AspNetCore.Mvc;
using Presence_API.Services.Completion;
using Presence_API.Services.Memory;

namespace Presence_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : ControllerBase
    {
        private readonly ILogger<ResponseController> _logger;
        private readonly ICompletionService _completionService;
        private readonly IMemoryService _memoryService;

        public ResponseController(ILogger<ResponseController> logger, ICompletionService completionService, IMemoryService memoryService)
        {
            _logger = logger;
            _completionService = completionService; 
            _memoryService = memoryService; 
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var memory = _memoryService.AddToMemory(Character.Chat,"what will you play today?");
                var response = await _completionService.CompleteAsync(memory);
                var firstTextResponse =  response.Choices.FirstOrDefault()?.Text;
                _memoryService.AddToMemory(Character.Sara, firstTextResponse);
                Console.WriteLine(_memoryService.GetMemory());
                return Ok(firstTextResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
