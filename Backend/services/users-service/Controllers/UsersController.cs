using System.Threading.Tasks;
using ExpensesManager.Common.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesManager.UsersService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IKafkaProducer _producer;

        public UsersController(IKafkaProducer producer)
        {
            _producer = producer;
        }

        [HttpGet("health")]
        public IActionResult Health() => Ok(new { status = "ok" });

        public record UserDto(int UserId, string Username, string? Email);

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserDto dto)
        {
            var ev = new { eventType = "UserCreated", payload = dto };
            await _producer.ProduceAsync("UserCreated", ev);
            return Created(string.Empty, dto);
        }
    }
}
