using Confluent.Kafka;
using KafkaExample3.Queue.Messages;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;

namespace KafkaExample3.Queue.Core
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
        }

        public async Task SendKafkaAsync(T message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.Brokers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                _producer = new ProducerBuilder<Null, string>(config).Build();
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
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.Brokers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                _producer = new ProducerBuilder<Null, string>(config).Build();
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
