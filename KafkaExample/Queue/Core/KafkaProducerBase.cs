using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;

namespace KafkaExample.Queue.Core
{
    public abstract class KafkaProducerBase<T> : IDisposable
    {
        private IProducer<Null,string> _producer;
        private readonly KafkaSettings _kafkaSettings;
        protected string _topic;
        public KafkaProducerBase(IOptions<KafkaSettings> options,string topic)
        {
            _kafkaSettings = options.Value;
            _topic = topic;

            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.Brokers,
                ClientId = Dns.GetHostName()
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendKafkaAsync(T message)
        {
            try
            {
                await _producer.ProduceAsync(_topic, new Message<Null, string>
                {
                    Value = Newtonsoft.Json.JsonConvert.SerializeObject(message)
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured: {ex.Message}");
            }
        }
        public void SendKafka(T message)
        {
            try
            {
                _producer.Produce(_topic, new Message<Null, string>
                {
                    Value = Newtonsoft.Json.JsonConvert.SerializeObject(message)
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured: {ex.Message}");
            }
        }
        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}
