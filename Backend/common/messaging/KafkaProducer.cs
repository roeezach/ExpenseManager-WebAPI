using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ExpensesManager.Common.Messaging
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly string _brokers;

        public KafkaProducer(ILogger<KafkaProducer> logger, IConfiguration config)
        {
            _logger = logger;
            _brokers = config["KAFKA_BROKERS"] ?? "localhost:9092";
        }

        public Task ProduceAsync(string topic, object message, CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(message);
            _logger.LogInformation("[KafkaProducer] ProduceAsync -> brokers={Brokers} topic={Topic} message={Message}", _brokers, topic, json);
            // No-op implementation for now (dev-friendly). Replace with real Kafka client when ready.
            return Task.CompletedTask;
        }
    }
}
