using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace KafkaExample.Queue.Core
{
    public abstract class KafkaConsumerBase<T> : BackgroundService
    {
        protected string _topic;
        private readonly IConsumer<string, string> _kafkaConsumer;
        private readonly KafkaMessage<T> _kafkaMessage;
        public KafkaConsumerBase(IOptions<KafkaSettings> options, string topic)
        {
            _topic= topic;
            var config = new ConsumerConfig
            {
                GroupId= options.Value.GroupId,
                BootstrapServers = options.Value.Brokers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _kafkaConsumer = new ConsumerBuilder<string, string>(config).Build();
            _kafkaMessage = new KafkaMessage<T>();
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
                    
                    _kafkaMessage.message=Newtonsoft.Json.JsonConvert.DeserializeObject<T>(cr.Message.Value);
                    _kafkaMessage.timestamp = cr.Message.Timestamp;
                    _kafkaMessage.key = cr.Message.Key;
                    _kafkaMessage.headers = cr.Message.Headers;

                    var message =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<T>(cr.Message.Value);
                    HandleMessage(_kafkaMessage);
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

        public abstract Task HandleMessage(KafkaMessage<T> message);

        public override void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();

            base.Dispose();
        }
    }
}
