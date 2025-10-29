using System.Threading;
using System.Threading.Tasks;

namespace ExpensesManager.Common.Messaging
{
    public interface IKafkaProducer
    {
        Task ProduceAsync(string topic, object message, CancellationToken cancellationToken = default);
    }
}
