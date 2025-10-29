using System.Threading;
using System.Threading.Tasks;
using ExpensesManager.Common.Messaging;
using ExpensesManager.UsersService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace UsersService.Tests
{
    class RecordingProducer : IKafkaProducer
    {
        public bool WasCalled { get; private set; }
        public Task ProduceAsync(string topic, object message, CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }

    public class UsersControllerTests
    {
        [Fact]
        public void Health_returns_ok()
        {
            var controller = new UsersController(new RecordingProducer());
            var result = controller.Health();
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, ok.StatusCode);
        }

        [Fact]
        public async Task SignUp_returns_created_and_calls_producer()
        {
            var producer = new RecordingProducer();
            var controller = new UsersController(producer);
            var dto = new UsersController.UserDto(1, "alice", "a@a.com");
            var res = await controller.SignUp(dto);
            var created = Assert.IsType<CreatedResult>(res);
            Assert.Equal(dto, created.Value);
            Assert.True(producer.WasCalled);
        }
    }
}
