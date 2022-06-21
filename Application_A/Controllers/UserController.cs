using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Application_A.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly ILogger<UserController> _logger;

        public UserController(IRabbitMQService rabbitMQService, ILogger<UserController> logger)
        {
            _rabbitMQService = rabbitMQService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendUser([FromBody] User u)
        {
            await _rabbitMQService.SendUserAsync(u);
            return Ok();
        }
    }
}
