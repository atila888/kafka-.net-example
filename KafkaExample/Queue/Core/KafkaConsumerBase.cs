using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace KafkaExample3.Queue.Core
{
    public abstract class KafkaConsumerBase<T> : BackgroundService
    {
        protected string _topic;
        private readonly IConsumer<Null, string> _kafkaConsumer;
        public KafkaConsumerBase(IOptions<KafkaSettings> options, string topic)
        {
            _topic= topic;
            var config = new ConsumerConfig
            {
                GroupId= options.Value.GroupId,
                BootstrapServers = options.Value.Brokers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _kafkaConsumer = new ConsumerBuilder<Null, string>(config).Build();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }
        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _kafkaConsumer.Consume(cancellationToken);

                    var message =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<T>(cr.Message.Value);
                    HandleMessage(message);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    Debug.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Unexpected error: {e}");
                    break;
                }
            }
        }

        public abstract Task HandleMessage(T message);

        public override void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();

            base.Dispose();
        }
    }
}
