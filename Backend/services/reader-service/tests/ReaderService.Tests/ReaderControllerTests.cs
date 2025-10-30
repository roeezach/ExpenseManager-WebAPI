using System.Threading.Tasks;
using ExpensesManager.ReaderService.Controllers;
using ExpensesManager.ReaderService.Models;
using ExpensesManager.ReaderService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ReaderService.Tests
{
    class RecordingProducer : ExpensesManager.Common.Messaging.IKafkaProducer
    {
        public bool WasCalled { get; private set; }
        public Task ProduceAsync(string topic, object message, System.Threading.CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }

    public class ReaderControllerTests
    {
        [Fact]
        public async Task UploadMetadata_calls_service_and_returns_created()
        {
            var inMemoryOptions = new DbContextOptionsBuilder<ExpensesManager.ReaderService.Data.ReaderDbContext>()
                .UseInMemoryDatabase("test_reader_controller_db").Options;

            using var ctx = new ExpensesManager.ReaderService.Data.ReaderDbContext(inMemoryOptions);
            var producer = new RecordingProducer();
            var svc = new ExpenseReadService(ctx, producer);
            var controller = new ReaderController(svc);

            var dto = new UploadedFile { Id = 0, UserID = 5, FileName = "x.csv", UploadDate = System.DateTime.UtcNow, FileType = "csv", LinkedMonth = 9, LinkedYear = 2025 };
            var res = await controller.UploadMetadata(dto);
            var created = Assert.IsType<CreatedResult>(res);
            Assert.NotNull(created.Value);
            Assert.True(producer.WasCalled);
        }
    }
}
