using Confluent.Kafka;

namespace KafkaExample.Queue.Core
{
    public class KafkaMessage<T>
    {
        public T message { get; set; }
        public Timestamp timestamp { get; set; }
        public string key { get; set; }
        public Headers headers { get; set; }
    }
}
